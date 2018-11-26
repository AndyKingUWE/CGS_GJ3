using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoliageSpawner : MonoBehaviour {


    [SerializeField] private MeshCollider collider;
    [SerializeField] private List<GameObject> prefabs;
    private bool stop;
    private List<GameObject> trees = new List<GameObject>();
    int counter = 0;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("CR");
            StartCoroutine(SpawnMaximumTrees());
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            foreach (var item in trees)
            {
                Destroy(item);
            }
            trees.Clear();
        }
    }

    public void SpawnTrees()
    {
        StartCoroutine(SpawnMaximumTrees());
    }

    IEnumerator SpawnMaximumTrees()
    {
        stop = false;
        while (!stop)
        {
            if (counter > 10)
            {
                yield break;
            }
            SpawnRandomObject();
            yield return new WaitForSeconds(Random.Range(0f,0.1f));
        }
    }

    void SpawnRandomObject()
    {
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        StartCoroutine(Spawn(prefab));
        
    }


    IEnumerator Spawn(GameObject prefab)
    {
        var bounds = collider.sharedMesh.bounds;
        var readyToSpawn = true;
        var position = Vector3.zero;
            readyToSpawn = true;
            var newX = Random.Range(bounds.min.x*2, bounds.max.x * 2);
            var newZ = Random.Range(bounds.min.z * 2, bounds.max.z * 2);
            var newY = bounds.max.y *2;

            position = new Vector3(newX, newY, newZ) + transform.position ;

            position = collider.ClosestPoint(position);

            RaycastHit[] hits = Physics.SphereCastAll(new Ray(position, new Vector3(1, 1, 1)), 1);
            foreach (var item in hits)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer("SpawnedObject"))
                {
                    Debug.Log(item.collider.gameObject);
                    readyToSpawn = false;
                    counter++;
                    yield break;
                }

            }


        var go = Instantiate(prefab,transform);
        go.transform.position = position;
        go.GetComponent<SpawnedObject>().desiredScale = Vector3.one * Random.Range(0.8f, 1.2f);
        trees.Add(go);
        //go.transform.SetParent(transform);
    }
   
}
