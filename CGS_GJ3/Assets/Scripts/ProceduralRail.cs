using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class ProceduralRail : MonoBehaviour {

    GameObject piece;

    bool used = false; 

	// Use this for initialization
	void Start () {
        piece = transform.parent.GetComponent<Original>().originPiece; 
        //piece = GameObject.Find("Origin").GetComponent<Original>().originPiece; 
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
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    CreateNewPiece("backLeft", 0);
            //    used = true;
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    CreateNewPiece("backRight", 0);
            //    used = true;
            //}
        }
    }

    public void CreateNewPiece(string _direction, int model)
    {
        //Move tile relative to the size of the tile
        Vector3 size = transform.parent.GetComponent<Original>().size;
        switch (_direction)
        {
            case "forward":
                Instantiate(piece, Vector3.Scale(transform.position + size,
                                                 transform.forward), 
                                                 transform.rotation, 
                                                 transform.parent);
                Debug.Log(size);
                Debug.Log(transform.forward); 

                break;
            case "left":
                Vector3 posL = size * 0.5f;
                posL += transform.position;
                posL.x *= transform.forward.x;
                posL.z *= transform.forward.z;
                posL.y *= transform.forward.y;
                //Vector3 posL2 = size * 0.75f;
                //posL2.x *= -transform.right.x;
                //posL2.z *= -transform.right.z;
                //GameObject obj = Instantiate(piece, transform.parent, false);
                //obj.transform.position = ((transform.position * transform.forward * (size * 0.5f)),
                //                                           transform.position.y,
                //                                           transform.position.z + (size.z * 0.75f));
                //posL += posL2;
                GameObject obj = Instantiate(piece, posL, transform.rotation,
                                                transform.parent);
                //Quaternion rot = transform.rotation;
                //rot.y -= 60;
                //obj.transform.rotation = rot;

                break;
            case "right":
                //Rotate Right 
                Vector3 rot = new Vector3(0, 60f);
                transform.Rotate(rot);
                //Go forward facing right
                Instantiate(piece, Vector3.Scale(transform.position + size,
                                                 transform.forward),
                                                 transform.rotation,
                                                 transform.parent);
                //Return to original Orientation
                transform.Rotate(-rot);


                //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y - 60f, transform.rotation.z, transform.rotation.w);


                //Vector3 tot = transform.position + size;
                //tot = Vector3.Scale(tot, transform.right);
                //Instantiate(piece, tot, Quaternion.Euler(new Vector3(transform.rotation.x,
                //                                                     transform.rotation.y + 60f,
                //                                                     transform.rotation.z)), 
                //                                                     transform.parent);
                //x0.5 z0.75
                //Instantiate(piece, Vector3.Scale(transform.position + size,
                //                                 transform.forward),
                //                                 transform.rotation,
                //                                 transform.parent);

                break;
            default:
                Debug.Log("Invalid Direction"); 
                break; 
        }
        
    }
}
