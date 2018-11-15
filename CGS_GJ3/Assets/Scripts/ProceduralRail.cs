using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRail : MonoBehaviour {

    public GameObject piece;

    bool used = false; 

	// Use this for initialization
	void Start () {
        //CreateNewPiece("left", 0);
        //CreateNewPiece("forward", 0);
        //CreateNewPiece("right", 0); 
	}
	
	// Update is called once per frame
	void Update () {
        //This is just for debugging 
        if (!used)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                CreateNewPiece("forward", 0);
                used = true; 
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateNewPiece("left", 0);
                used = true; 
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateNewPiece("right", 0);
                used = true; 
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                CreateNewPiece("backLeft", 0);
                used = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CreateNewPiece("backRight", 0);
                used = true;
            }
        }
    }

    public void CreateNewPiece(string _direction, int model)
    {
        Vector3 size = GetComponent<MeshRenderer>().bounds.size;
        switch (_direction)
        {
            case "forward":
                Instantiate(piece, new Vector3((transform.position.x + size.x),
                                                transform.position.y,
                                                transform.position.z), Quaternion.identity);
                break;
            case "left":
                Instantiate(piece, new Vector3((transform.position.x + size.x * 0.5f),
                                                transform.position.y,
                                                transform.position.z + size.z * 0.75f), Quaternion.identity);
                break;
            case "right":
                Instantiate(piece, new Vector3((transform.position.x + size.x * 0.5f),
                                               transform.position.y,
                                               transform.position.z - size.z * 0.75f), Quaternion.identity);
                break;
            case "backLeft":
                Instantiate(piece, new Vector3((transform.position.x - size.x * 0.5f),
                                                transform.position.y,
                                                transform.position.z + size.z * 0.75f), Quaternion.identity);
                break;
            case "backRight":
                Instantiate(piece, new Vector3((transform.position.x - size.x * 0.5f),
                                               transform.position.y,
                                               transform.position.z - size.z * 0.75f), Quaternion.identity);
                break;
            default:
                Debug.Log("Invalid Direction"); 
                break; 
        }
        
    }
}
