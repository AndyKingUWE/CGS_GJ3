using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    [Header("Tile Prefabs")]
    public GameObject StartingPrefab;
    public List<GameObject> ForwardPrefabs;
    public List<GameObject> LeftPrefabs;
    public List<GameObject> RightPrefabs;
    public List<GameObject> DecorativePrefabs;

    public GameObject ForwardTrackPrefab;
    public GameObject RightTrackPrefab;
    public GameObject LeftTrackPrefab;


    [HideInInspector]
    public Vector3 size;
    [HideInInspector]
    public bool waitForPreviousTile = true;
    

    /// <summary>
    /// Reference to the handcar 
    /// </summary>
    public HandCar HandCarRef;
    public GameObject HandCarPrefab;


    [Header("Track spawner settings")]
    //Speed individual tracks fall 
    public float trackLaySpeed = 0.1f;
    //Speed tracks lay
    public float trackLayFrequency = 1f;
    //Max tracks laid per second
    public int trackLayMaxFreq = 1;
    public float heightUp = 5f;
    public int deathTime = 5;



    // Use this for initialization
    void Start () {
        size = StartingPrefab.GetComponent<Renderer>().bounds.size;
        Debug.Log(size);
        var obj = Instantiate(StartingPrefab, Vector3.zero, Quaternion.identity, transform);
        var tile = obj.GetComponent<Tile>();
        tile.firstTile = true;
        tile.FirstTile();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
