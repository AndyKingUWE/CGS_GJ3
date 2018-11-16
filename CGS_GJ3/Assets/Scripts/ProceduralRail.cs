using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class ProceduralRail : MonoBehaviour {

    public Original origin;
    GameObject pieceF;
    GameObject pieceL;
    GameObject pieceR;
    float aliveTime = 0; 
    int deathTime = 5; 
    bool used = false; 

	// Use this for initialization
	void Start () {
        pieceF = origin.forwardPieces[Random.Range(0, origin.forwardPieces.Length - 1)];
        pieceL = origin.forwardPieces[Random.Range(0, origin.forwardPieces.Length - 1)];
        pieceR = origin.forwardPieces[Random.Range(0, origin.forwardPieces.Length - 1)];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "cart")
            return; 

        if (!used)
        switch (Random.Range(0,2))
        {
            case 0:
                CreateNewPiece("forward", pieceF);
                used = true;
                break;
            case 1:
                CreateNewPiece("left", pieceL);
                used = true;
                break;
            case 2:
                CreateNewPiece("right", pieceR);
                used = true;
                break;
            default:
                Debug.Log("Trigger switch broken"); 
                break;
        }
    }

    public void SetOrigin(Original _origin)
    {
        origin = _origin; 
    }
	
	// Update is called once per frame
	void Update () {
        aliveTime += Time.deltaTime; 
        //This is just for debugging 
        if (!used)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                CreateNewPiece("forward", pieceF);
                used = true; 
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateNewPiece("left", pieceL);
                used = true; 
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateNewPiece("right", pieceR);
                used = true; 
            }
        }
        if (used && aliveTime > deathTime)
        {
            Destroy(gameObject); 
        }
    }

    public void CreateNewPiece(string _direction, GameObject _model)
    {
        //Move tile relative to the size of the tile
        Vector3 size = origin.size;
        Debug.Log("Size: " + size);
        Debug.Log("Transform Forward: " + transform.forward);
        //Reduce x by 25% as hexagons are not uniform
        Vector3 trForward = new Vector3(transform.forward.x * 0.75f, transform.forward.y, transform.forward.z);
        switch (_direction)
        {
            case "forward":
               var obj = Instantiate(_model, transform.position + Vector3.Scale(size, trForward), 
                                   transform.rotation, 
                                   transform.parent);
                obj.GetComponent<ProceduralRail>().SetOrigin(origin);
                break;
            case "left":
                var objL = Instantiate(_model, transform.position + Vector3.Scale(size, trForward),
                   Quaternion.Euler(transform.rotation.eulerAngles.x,
                                    transform.rotation.eulerAngles.y - 60,
                                    transform.rotation.eulerAngles.z),
                                    transform.parent);
                objL.GetComponent<ProceduralRail>().SetOrigin(origin);

                break;
            case "right":
                var objR = Instantiate(_model, transform.position + Vector3.Scale(size, trForward),
                   Quaternion.Euler(transform.rotation.eulerAngles.x,
                                    transform.rotation.eulerAngles.y + 60,
                                    transform.rotation.eulerAngles.z),
                                    transform.parent);
                objR.GetComponent<ProceduralRail>().SetOrigin(origin);
                break;
            default:
                Debug.Log("Invalid Direction"); 
                break; 
        }
        
    }
}
