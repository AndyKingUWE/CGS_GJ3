using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Original : MonoBehaviour
{
    public GameObject originPiece;
    public GameObject[] forwardPieces;
    public GameObject[] leftPieces;
    public GameObject[] rightPieces;
    [HideInInspector]
    public Vector3 size;

    private void Start()
    {
        //size = originPiece.GetComponent<MeshRenderer>().bounds.size;
        size = originPiece.GetComponent<Renderer>().bounds.size; 
        var obj = Instantiate(originPiece, Vector3.zero, Quaternion.identity, transform);
        obj.GetComponent<ProceduralRail>().SetOrigin(this); 
    }
}
