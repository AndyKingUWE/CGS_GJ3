using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoliageSpawner : MonoBehaviour {


    [SerializeField] private MeshCollider collider;
    [SerializeField] private List<GameObject> buildingPrefabs;
    [SerializeField] private List<GameObject> fieldPrefabs;
    [SerializeField] private List<GameObject> treePrefabs;
    [SerializeField] private List<GameObject> foliagePrefabs;
    private bool stop;
    private List<GameObject> trees = new List<GameObject>();
    public Tile currentTile;
    int counter = 0;
    int spawned = 0;
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
        if (Input.GetKeyUp(KeyCode.F))
        {
            StartCoroutine(SpawnBuildingCR());
            Debug.Log("CR");
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

    IEnumerator SpawnBuildingCR()
    {
        spawned = 0;
        counter = 0;
        stop = false;
        while (!stop)
        {
            if (counter > 2 || spawned > 0)
            {
                SpawnFields();
                yield break;
            }
            var prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];

            StartCoroutine(Spawn(prefab));
            yield return new WaitForSeconds(Random.Range(0f, 0.01f));
        }

    }

    public void SpawnTrees()
    {
        StartCoroutine(SpawnMaximumTrees());
    }

    public void SpawnFields()
    {
        StartCoroutine(SpawnFieldCR());
    }

    IEnumerator SpawnFieldCR()
    {
        spawned = 0;
        counter = 0;
        stop = false;
        while (!stop)
        {
            if (counter > 2 || spawned > 2)
            {
                StartCoroutine(SpawnMaximumTrees());
                yield break;
            }
            SpawnField();
            yield return new WaitForSeconds(Random.Range(0f, 0.01f));
        }

    }

    IEnumerator SpawnMaximumTrees()
    {
        counter = 0;
        spawned = 0;
        stop = false;
        while (!stop)
        {
            if (counter > 25)
            {
                StartCoroutine(SpawnGrass());
                yield break;
            }
            SpawnRandomTree();
            yield return new WaitForSeconds(Random.Range(0f, 0.01f));
        }

    }
    void SpawnField()
    {
        var prefab = fieldPrefabs[Random.Range(0, fieldPrefabs.Count)];
        StartCoroutine(Spawn(prefab));

    }

    IEnumerator SpawnGrass()
    {
        counter = 0;
        spawned = 0;
        stop = false;
        while (!stop)
        {
            if (counter > 10)
            {
                yield break;
            }
            SpawnRandomFoliage();
            yield return new WaitForSeconds(Random.Range(0f, 0.01f));
            //yield return new WaitForSeconds(Random.Range(0f, 0.1f));
        }
    }

    void SpawnRandomFoliage()
    {
        var prefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
        StartCoroutine(Spawn(prefab));

    }


    void SpawnRandomTree()
    {
        var prefab = treePrefabs[Random.Range(0, treePrefabs.Count)];
        StartCoroutine(Spawn(prefab));
        
    }


    IEnumerator Spawn(GameObject prefab)
    {
        var bounds = collider.sharedMesh.bounds;
        //var objbounds = prefab.GetComponent<SpawnedObject>().myCollider.bounds.extents ;
        //Debug.Log(objbounds);
        var readyToSpawn = true;
        var position = Vector3.zero;
            readyToSpawn = true;
            var newX = Random.Range(bounds.min.x*1.8f, bounds.max.x * 1.8f);
            var newZ = Random.Range(bounds.min.z * 1.8f, bounds.max.z * 1.8f);
            var newY = bounds.max.y *2;

            position = new Vector3(newX, newY, newZ) + transform.position ;

            position = collider.ClosestPoint(position);

            //RaycastHit[] hits = Physics.BoxCastAll(position, objbounds, Vector3.one);
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(position, new Vector3(1, 1, 1)), 1.5f);
            foreach (var item in hits)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer("SpawnedObject"))
                {
                    readyToSpawn = false;
                    counter++;
                    yield break;
                }

            }


        var go = Instantiate(prefab,transform);
        yield return new WaitForEndOfFrame();
        go.transform.position = position;
        go.GetComponent<SpawnedObject>().audioSource = currentTile.audioSource;
        go.GetComponent<SpawnedObject>().PlaySound();
        go.GetComponent<SpawnedObject>().desiredScale = Vector3.one * Random.Range(0.8f, 1.2f);
        spawned++;
        trees.Add(go);
        //go.transform.SetParent(transform);
    }
   
}
