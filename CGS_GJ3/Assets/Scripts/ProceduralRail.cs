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
    float timePassed = 0; 
    int counter = 0;
    
    bool used = false;
    bool finishedRailPlacing = false; 

    public GameObject[] tracks; 
    

	// Use this for initialization
	void Start () {
        pieceF = origin.forwardPieces[Random.Range(0, origin.forwardPieces.Length - 1)];
        pieceL = origin.leftPieces[Random.Range(0, origin.leftPieces.Length - 1)];
        pieceR = origin.rightPieces[Random.Range(0, origin.rightPieces.Length - 1)];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        // The faster you go the smaller speed is (Because smaller speed = faster)
        origin.speed = 1 / ((other.attachedRigidbody.velocity.magnitude * 5) / 100);
        if (origin.speed > 1)
            origin.speed = 1;
        if (origin.speed < 0.1)
            origin.speed = 0.1f;

        if (!used)
        switch (0)
                //Random.Range(0, 2)
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
	void Update ()
    {
        //aliveTime += Time.deltaTime;
        timePassed += Time.deltaTime; 
        //This is just for debugging 
        CheckInput(); 
        
        //Delete past tracks
        //if (used && aliveTime > deathTime)
        //{
        //    Destroy(gameObject); 
        //}

        if(!finishedRailPlacing)
            UpdateRailFalling();

    }

    void UpdateRailFalling()
    {
        if (timePassed > origin.speed)
        {
            tracks[counter].SetActive(true);
            timePassed = 0;
            counter++;
            if (counter == tracks.Length)
                finishedRailPlacing = true; 
        }
    }

    void CheckInput()
    {
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
