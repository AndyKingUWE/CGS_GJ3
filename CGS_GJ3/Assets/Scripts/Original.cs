using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Original : MonoBehaviour
{
    public GameObject originPiece;
    public GameObject[] forwardPieces;
    public GameObject[] leftPieces;
    public GameObject[] rightPieces;
    public GameObject track_straight_new;
    public GameObject handCar; 
    [HideInInspector]
    public Vector3 size;
    [HideInInspector]
    public HandCar handCarSpeed;
    [HideInInspector]
    public bool waitForPreviousTile = true;
    //Speed individual tracks fall 
    public float trackLaySpeed = 0.1f;
    //Speed tracks lay
    public float trackLayFrequency = 1f;
    //Max tracks laid per second
    public int trackLayMaxFreq = 1;
    public float heightUp = 5f;
    public int deathTime = 5;

    private void Start()
    {
        //size = originPiece.GetComponent<MeshRenderer>().bounds.size;
        size = originPiece.GetComponent<Renderer>().bounds.size; 
        var obj = Instantiate(originPiece, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<ProceduralRail>().SetOrigin(this);
        obj.GetComponent<ProceduralRail>().firstTile = true;

    }
}
