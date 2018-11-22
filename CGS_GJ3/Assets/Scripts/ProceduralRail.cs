using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class ProceduralRail : MonoBehaviour {

    public Original origin;
    public bool firstTile = false; 
    GameObject pieceF;
    GameObject pieceL;
    GameObject pieceR;
    float tracksLaidfreq = 0;
    float distanceTravelled = 50;
    int counter = 0;
    //Tracks per second count
    int tpsCount = 0; 
    Vector3 startTrackPos = new Vector3(0, 2.45f, -19.64f);
    Vector3 startHandCarPos = new Vector3(0, 5.7f, -41.5f);
    
    bool used = false;
    bool finishedRailPlacing = false;
    bool waitingForPreviousTile = true;
    int modulo = 5;
    //public GameObject[] tracks; 
    

	// Use this for initialization
	void Start () {
        pieceF = origin.forwardPieces[Random.Range(0, origin.forwardPieces.Length - 1)];
        pieceL = origin.leftPieces[Random.Range(0, origin.leftPieces.Length - 1)];
        pieceR = origin.rightPieces[Random.Range(0, origin.rightPieces.Length - 1)]; modulo = Random.Range(3, 10);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        // The faster you go the smaller speed is (Because smaller speed = faster)
       
            //origin.speed = 0.2f;
            //origin.speed = 1 / ((hc.speed * 5) / 100);
            //if (origin.speed > 1)
            //    origin.speed = 1;
            //if (origin.speed < 0.1)
            //    origin.speed = 0.1f;
        

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
        tracksLaidfreq = tpsCount / Time.deltaTime;
        tpsCount = 0; 
        
        //This is just for debugging 
        CheckInput();

        //Delete past tracks
        //if (used && aliveTime > deathTime)
        //{
        //    Destroy(gameObject); 
        //}

        //Make sure ahead tiles don't start putting down 
        //Track until previous tile has finished
        if (waitingForPreviousTile && !firstTile)
        {
            waitingForPreviousTile = origin.waitForPreviousTile;
            //If tile is allowed to lay track, hold next tile
            if (waitingForPreviousTile == false)
                origin.waitForPreviousTile = true;
            return;
        }
        

        if (!finishedRailPlacing)
            UpdateRailFalling();
        //Once finished laying tiles prompt next tile to 
        else
        {
            origin.waitForPreviousTile = false;
            enabled = false; 
        }

        distanceTravelled += (origin.handCarSpeed.input*Mathf.PI) * Time.deltaTime;
        //Debug.Log(distanceTravelled);
    }

    void UpdateRailFalling()
    {
        if (firstTile && counter == 0)
        {
            //Put down track for Hand car to start on 
            GameObject segment = Instantiate(origin.track_straight_new, transform.GetChild(0));
            FallSpawner[] fallspawners = segment.transform.GetComponentsInChildren<FallSpawner>();

            //Set first track to not come down one at a time
            foreach (FallSpawner fs in fallspawners)
            {
                fs.delay = 0;
            }

            segment.transform.localPosition = startTrackPos;
            segment.transform.localScale /= 2;
            startTrackPos += new Vector3(0, 0, 2);
            //Place handcar 
            GameObject handCar = Instantiate(origin.handCar, transform.parent);
            handCar.transform.localPosition = startHandCarPos;
            origin.handCarSpeed = handCar.GetComponent<HandCar>();
            counter++; 
        }
        if (distanceTravelled > origin.trackLayFrequency && tracksLaidfreq < origin.trackLayMaxFreq)
        {
            if (counter == 7)
                PlaceTurnTrack();
            else
                PlaceStraightTrack(); 

            distanceTravelled = 0; 
            counter++;
            tpsCount++;
            if (counter == 22)
                finishedRailPlacing = true;
        }
    }

    void PlaceTurnTrack()
    {
        startTrackPos += new Vector3(-7.23F, 0, 6.3F);
        GameObject segment = Instantiate(origin.track_bent_new, transform.GetChild(0));
        segment.transform.localPosition = startTrackPos;
        FallSpawner[] fallspawners = segment.transform.GetChild(0).GetComponentsInChildren<FallSpawner>();
        segment.transform.localScale /= 2;
        startTrackPos += new Vector3(0, 0, 2);
        for (int i = 0; i < fallspawners.Length; i++)
        {
            fallspawners[i].delay = (i / 20f) + 0.1f;
        }
        if (counter % modulo == 0)
        {
            segment.transform.localPosition -= Vector3.up * 0.01f * Random.Range(5, 10);
            modulo = Random.Range(3, 10);
        }
        counter += 7; 
    }

    void PlaceStraightTrack()
    {
        GameObject segment = Instantiate(origin.track_straight_new, transform.GetChild(0));
        segment.transform.localPosition = startTrackPos;
        FallSpawner[] fallspawners = segment.transform.GetChild(0).GetComponentsInChildren<FallSpawner>();
        segment.transform.localScale /= 2;
        startTrackPos += new Vector3(0, 0, 2);
        for (int i = 0; i < fallspawners.Length; i++)
        {
            fallspawners[i].delay = (i / 20f) + 0.1f;
        }
        if (counter % modulo == 0)
        {
            segment.transform.localPosition -= Vector3.up * 0.01f * Random.Range(5, 10);
            modulo = Random.Range(3, 10);
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
                //Decorative Pieces
                Instantiate(origin.decorativePieces[Random.Range(0, origin.decorativePieces.Length)],
                            obj.transform.position - new Vector3(size.x * 0.75f, Random.Range(-1f, 1f), size.z * 0.5f),
                            obj.transform.rotation, 
                            obj.transform.parent);
                Instantiate(origin.decorativePieces[Random.Range(0, origin.decorativePieces.Length)],
                            obj.transform.position - new Vector3(-size.x * 0.75f, Random.Range(-1f, 1f), size.z * 0.5f),
                            obj.transform.rotation,
                            obj.transform.parent);
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
