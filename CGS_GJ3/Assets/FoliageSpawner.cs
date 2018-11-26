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
	// Use this for initialization
	void Start () {

        Debug.Log("CR");
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

    IEnumerator SpawnMaximumTrees()
    {
        stop = false;
        while (!stop)
        {
            SpawnRandomObject();
            yield return new WaitForSeconds(Random.Range(0f,0.1f));
        }
    }

    void SpawnRandomObject()
    {
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        Debug.Log("CR");
        StartCoroutine(Spawn(prefab));
        
    }


    IEnumerator Spawn(GameObject prefab)
    {
        Debug.Log("CR");
        var bounds = collider.sharedMesh.bounds;
        var readyToSpawn = true;
        int counter = 0;
        var position = Vector3.zero;
        do
        {
            readyToSpawn = true;
            var newX = Random.Range(bounds.min.x*2, bounds.max.x * 2);
            var newZ = Random.Range(bounds.min.z * 2, bounds.max.z * 2);
            var newY = bounds.max.y *2;

            position = new Vector3(newX, newY, newZ) + transform.position ;

            position = collider.ClosestPoint(position);

            RaycastHit[] hits = Physics.SphereCastAll(new Ray(position, new Vector3(1, 1, 1)), 1);
            Debug.Log(hits.Length);
            foreach (var item in hits)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer("SpawnedObject"))
                {
                    Debug.Log(item.collider.gameObject);
                    readyToSpawn = false;
                    counter++;
                    if(counter>3)
                    {
                        stop = true;
                        yield break;
                    }
                    break;
                }

            }

            yield return null;

        } while (!readyToSpawn);

        var go = Instantiate(prefab);
        go.transform.position = position;
        trees.Add(go);
        //go.transform.SetParent(transform);
    }
   
}
