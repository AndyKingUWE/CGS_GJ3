using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    public enum DIRECTION
    {
        FORWARD = 0,
        RIGHT = 2,
        LEFT = 1
    }

    public enum BIOME
    {
        TEMPERATE = 0
    }

    //private GameObject forwardPiece;
    //private GameObject forwardRightPiece;
    //private GameObject forwardLeftPiece;
    private DIRECTION direction;
    public DIRECTION PreviousDirection;
    public bool firstTile = false;



    //todo: refactor & check
    private float tracksLaidfreq = 0;
    int tpsCount = 0;
    bool finishedRailPlacing = false;
    int counter = 0;
    int modulo = 5;
    Vector3 startTrackPos = new Vector3(0, 2.45f, -19.64f);
    Vector3 startHandCarPos = new Vector3(0, 5.7f, -41.5f);
    float distanceTravelled = 0;
    bool waitingForPreviousTile = true; 
    private TileManager tileManager;

    private bool initialised;


    // Use this for initialization
    void Start()
    {
        tileManager = TileManager.instance;
        modulo = Random.Range(3, 10);
    }


    // Update is called once per frame
    void Update()
    {
        //aliveTime += Time.deltaTime;
        tracksLaidfreq = tpsCount / Time.deltaTime;
        tpsCount = 0;

       

        //Delete past tracks
        //if (used && aliveTime > deathTime)
        //{
        //    Destroy(gameObject); 
        //}

        //Make sure ahead tiles don't start putting down 
        //Track until previous tile has finished
        if (waitingForPreviousTile && !firstTile)
        {
            waitingForPreviousTile = tileManager.waitForPreviousTile; 
            //If tile is allowed to lay track, hold next tile
            if (waitingForPreviousTile == false)
                tileManager.waitForPreviousTile = true;
            return; 
        }


        if (!finishedRailPlacing)
            UpdateRailFalling();
        //Once finished laying tiles prompt next tile to 
        else
        {
            tileManager.waitForPreviousTile = false;
            enabled = false;
        }
        if(tileManager.HandCarRef)
            distanceTravelled += (tileManager.HandCarRef.input * Mathf.PI) * Time.deltaTime;
        //Debug.Log(distanceTravelled);
    }


    public void FirstTile()
    {
        tileManager = TileManager.instance;
        modulo = Random.Range(3, 10);
        //Put down tracks for Hand car to start on 
        for (int i = 0; i < 3; i++)
        {
            GameObject segment = Instantiate(tileManager.ForwardTrackPrefab, transform.GetChild(0));
            FallSpawner[] fallspawners = segment.transform.GetComponentsInChildren<FallSpawner>();

            //Set first track to not come down one at a time
            foreach (FallSpawner fs in fallspawners)
            {
                fs.delay = 0;
            }

            segment.transform.localPosition = startTrackPos;
            segment.transform.localScale /= 2;
            startTrackPos += new Vector3(0, 0, 2);
            counter++;
        }
        //Place handcar 
        GameObject handCar = Instantiate(tileManager.HandCarPrefab, transform.parent);
        handCar.transform.localPosition = startHandCarPos;
        tileManager.HandCarRef = handCar.GetComponent<HandCar>();
    }

    private void UpdateRailFalling()
    {
       
        if (distanceTravelled > tileManager.trackLayFrequency && tracksLaidfreq < tileManager.trackLayMaxFreq)
        {
            if (direction == DIRECTION.LEFT || direction == DIRECTION.RIGHT)
            {
                if (counter == 5)
                {
                    PlaceTurnTrack();

                }

                else
                {

                    PlaceStraightTrack();
                }
            }
            else
            {
                PlaceStraightTrack();
            }

            distanceTravelled = 0;
            counter++;
            tpsCount++;
            if (counter == 22)
                finishedRailPlacing = true;
        }
    }


    private void PlaceTurnTrack()
    {
        startTrackPos += new Vector3(-7.23F, 0, 6.3F);
        GameObject segment = Instantiate(tileManager.LeftTrackPrefab, transform.GetChild(0));
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

       
        startTrackPos = new Vector3(-11.801f,2.45f,6.681f);
        counter += 11;
    }


    private void PlaceStraightTrack()
    {
        GameObject segment = Instantiate(tileManager.ForwardTrackPrefab, transform.GetChild(0));
        if (direction == DIRECTION.LEFT && counter > 10)
        {
            Debug.Log("rotated!");
            segment.transform.Rotate(0, -60, 0);

        }
        segment.transform.localPosition = startTrackPos;
        
        FallSpawner[] fallspawners = segment.transform.GetChild(0).GetComponentsInChildren<FallSpawner>();
        segment.transform.localScale /= 2;
        startTrackPos += transform.InverseTransformDirection(segment.transform.forward) * 2;
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


        if (!initialised)
        {
            initialised = true;
            GenerateNextTile();
        }
    }


    private void GenerateNextTile()
    {
           var size = tileManager.size;
        //Reduce x by 25% as hexagons are not uniform

        GameObject prefab = null;
        float yrot = 0.0f;
        var newDir = DIRECTION.FORWARD;
        if (!firstTile)
        {
            Debug.Log(PreviousDirection);
            newDir = (DIRECTION)Random.Range(0, 2);
            switch (direction)
            {
                case DIRECTION.RIGHT:
                    yrot = 60;
                    break;
                case DIRECTION.LEFT:
                    yrot = -60;
                    break;
            }
        }

        var trForward =  Quaternion.Euler(0, yrot, 0) * transform.forward;
        if (yrot == 0)
        {
            //var rot = transform.rotation * Vector3.right * 0.75f;
            //trForward.Scale();
            trForward.x *= 0.75f;
            Debug.Log(trForward);
        }
        else
        {

            trForward.x *= 0.8675f;
            Debug.Log(trForward);
        }
        switch (newDir)
        {
            case DIRECTION.FORWARD:
                prefab = tileManager.ForwardPrefabs[Random.Range(0, tileManager.ForwardPrefabs.Count - 1)];

                var obj = Instantiate(prefab, transform.position + Vector3.Scale(size, trForward),
                    Quaternion.Euler(transform.rotation.eulerAngles.x,
                        transform.rotation.eulerAngles.y +yrot,
                        transform.rotation.eulerAngles.z),
                    transform.parent);
                obj.GetComponent<Tile>().PreviousDirection = direction;
                obj.GetComponent<Tile>().direction = newDir;

                //Decorative Pieces
                //Instantiate(tileManager.DecorativePrefabs[Random.Range(0, tileManager.DecorativePrefabs.Count)],
                //    obj.transform.position - new Vector3(size.x * 0.75f, 0, size.z * 0.5f),
                //    obj.transform.rotation,
                //    obj.transform.parent);
                //Instantiate(tileManager.DecorativePrefabs[Random.Range(0, tileManager.DecorativePrefabs.Count)],
                //    obj.transform.position - new Vector3(-size.x * 0.75f, 0, size.z * 0.5f),
                //    obj.transform.rotation,
                //    obj.transform.parent);


                break;
            case DIRECTION.RIGHT:
                prefab = tileManager.RightPrefabs[Random.Range(0, tileManager.LeftPrefabs.Count - 1)];
                var objL = Instantiate(prefab, transform.position + Vector3.Scale(size, trForward),
                    Quaternion.Euler(transform.rotation.eulerAngles.x,
                        transform.rotation.eulerAngles.y,
                        transform.rotation.eulerAngles.z),
                    transform.parent);
                objL.GetComponent<Tile>().PreviousDirection = direction;
                objL.GetComponent<Tile>().direction = newDir;
                break;
            case DIRECTION.LEFT:
                prefab = tileManager.RightPrefabs[Random.Range(0, tileManager.LeftPrefabs.Count - 1)];
                var objR = Instantiate(prefab, transform.position + Vector3.Scale(size, trForward),
                    Quaternion.Euler(transform.rotation.eulerAngles.x,
                        transform.rotation.eulerAngles.y + yrot,
                        transform.rotation.eulerAngles.z),
                    transform.parent);
                objR.GetComponent<Tile>().PreviousDirection = direction;
                objR.GetComponent<Tile>().direction = newDir;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (prefab == null)
        {
            return;
        }
        else
        {

        }


       

       

    }


}