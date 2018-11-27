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

    bool waitingForPreviousTile = true;

    //private GameObject forwardPiece;
    //private GameObject forwardRightPiece;
    //private GameObject forwardLeftPiece;
    public DIRECTION direction;
    public DIRECTION PreviousDirection;
    public bool firstTile = false;
    public bool spawnTracks = true;
    public bool AnimationFinished = false;
    private Animator animator;
    //todo: refactor & check
    private float tracksLaidfreq = 0;
    int tpsCount = 0;
    bool finishedRailPlacing = false;
    int counter = 0;
    int modulo = 5;
    Vector3 startTrackPos = new Vector3(0, 2.45f, -19.64f);
    Vector3 startHandCarPos = new Vector3(0, 5.7f, -41.5f);
    float distanceTravelled = 0;
    float waitForTurnTrack = 0; 
    private TileManager tileManager;

    private bool initialised;
    private FoliageSpawner fs;
    [SerializeField] private float distance = 0;

    // Use this for initialization
    void Start()
    {
        tileManager = TileManager.instance;
        modulo = Random.Range(3, 10);
        fs = GetComponentInChildren<FoliageSpawner>();
        animator = GetComponent<Animator>();
        if (fs!=null)
        {
            StartCoroutine(WaitForFinish());
        }
    }
    private IEnumerator WaitForFinish()
    {
        while (!AnimationFinished)
        {
            yield return new WaitForEndOfFrame();
        }
        animator.enabled = false;
        fs.SpawnTrees();
    }

    // Update is called once per frame
    void Update()
    {
        if(!AnimationFinished)
        {
            return;
        }
        //distance = Vector3.Distance(transform.position, tileManager.HandCarRef.transform.position);
        //if(distance>220)
        //{
        //    tileManager.RemoveTile(this);
        //}
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

        //    return;
        //}

        if (!spawnTracks)
            return;

        if (!finishedRailPlacing)
            UpdateRailFalling();
        //Once finished laying tiles prompt next tile to 
        else
        {
            tileManager.waitForPreviousTile = false;
           // enabled = false;
        }

        if (tileManager.HandCarRef)
            distanceTravelled +=  Time.deltaTime;
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
        AnimationFinished = true;
    }


    private void UpdateRailFalling()
    {
        if (waitForTurnTrack > 0)
            waitForTurnTrack -= Time.deltaTime;

        if (distanceTravelled > 1/tileManager.trackLayFrequency && tracksLaidfreq < tileManager.trackLayMaxFreq)
        {
            if (direction == DIRECTION.LEFT || direction == DIRECTION.RIGHT)
            {
                if (counter == 5)
                {
                    PlaceTurnTrack();
                }
                else if (waitForTurnTrack <= 0)
                {
                    PlaceStraightTrack();
                }
                else
                {
                    counter--;
                    tpsCount--;
                    Debug.Log(waitForTurnTrack); 
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
        GameObject segment = null;
        switch (direction)
        {
            case DIRECTION.FORWARD:
                break;
            case DIRECTION.RIGHT:
                segment = Instantiate(tileManager.RightTrackPrefab, transform.GetChild(0));
                break;
            case DIRECTION.LEFT:
                segment = Instantiate(tileManager.LeftTrackPrefab, transform.GetChild(0));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        segment.transform.localPosition = startTrackPos;
        FallSpawner[] fallspawners = segment.transform.GetChild(0).GetComponentsInChildren<FallSpawner>();
        segment.transform.localScale /= 2;
        startTrackPos += new Vector3(0, 0, 2);

        if (direction == DIRECTION.LEFT)
            for (int i = 0; i < fallspawners.Length; i++)
            {
                fallspawners[i].delay = (i / 20f) + 0.1f;
            }
        else if (direction == DIRECTION.RIGHT)
        {
            int x = 0; 
            for (int i = fallspawners.Length - 1; i >= 0; i--)
            {
                fallspawners[i].delay = (x / 20f) + 0.1f;
                x++; 
            }
        }

        waitForTurnTrack = (fallspawners.Length / 20f) + 0.1f; 
        if (counter % modulo == 0)
        {
            segment.transform.localPosition -= Vector3.up * 0.01f * Random.Range(5, 10);
            modulo = Random.Range(3, 10);
        }

        switch (direction)
        {
            case DIRECTION.FORWARD:
                break;
            case DIRECTION.RIGHT:
                startTrackPos = new Vector3(11.725f, 2.45f, 6.681f);
                break;
            case DIRECTION.LEFT:
                startTrackPos = new Vector3(-11.801f, 2.45f, 6.681f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        counter += 11;
        StartCoroutine(PlayLegoTileSound(8 , 0.2f));
    }

    private IEnumerator PlayLegoTileSound(int loop, float time)
    {
        for (int i = 0; i < loop; i++)
        {
            yield return new WaitForSeconds(time);
            switch (Random.Range(0, 3))
            {
                case 0:
                    SoundManager.instance.PlaySingle("Lego Building Sound Effect 1");
                    break;
                case 1:
                    SoundManager.instance.PlaySingle("Lego Building Sound Effect 2");
                    break;
                case 2:
                    SoundManager.instance.PlaySingle("Lego Building Sound Effect 3");
                    break;
                default:
                    break;

            }
        }
    }

    private void PlaceStraightTrack()
    {
        GameObject segment = Instantiate(tileManager.ForwardTrackPrefab, transform.GetChild(0));
        if (direction == DIRECTION.LEFT && counter > 10)
        {
            segment.transform.Rotate(0, -60, 0);
        }
        else if (direction == DIRECTION.RIGHT && counter > 10)
        {
            segment.transform.Rotate(0, 60, 0);
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
        StartCoroutine(PlayLegoTileSound(1, 0.3f));
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
        GameObject chosenTile = null; 
        //Reduce x by 25% as hexagons are not uniform

        GameObject prefab = null;
        float yrot = 0.0f;
        var newDir = DIRECTION.FORWARD;
        if (!firstTile)
        {
            Debug.Log(PreviousDirection);
            newDir = (DIRECTION) Random.Range(0, 3);
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

        var trForward = Quaternion.Euler(0, yrot, 0) * transform.forward;
        var trRight = Quaternion.Euler(0, yrot, 0) * transform.right;


        GameObject obj = null;
        switch (newDir)
        {
            case DIRECTION.FORWARD:
                prefab = tileManager.ForwardPrefabs[Random.Range(0, tileManager.ForwardPrefabs.Count)];
                chosenTile = prefab; 
                break;
            case DIRECTION.RIGHT:
                prefab = tileManager.RightPrefabs[Random.Range(0, tileManager.RightPrefabs.Count)];
                chosenTile = prefab;
                break;
            case DIRECTION.LEFT:
                prefab = tileManager.LeftPrefabs[Random.Range(0, tileManager.LeftPrefabs.Count)];
                chosenTile = prefab;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        obj = Instantiate(prefab, transform.position + trForward * 86.6f,
            Quaternion.Euler(transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + yrot,
                transform.rotation.eulerAngles.z),
            transform.parent);
        obj.GetComponent<Tile>().PreviousDirection = direction;
        obj.GetComponent<Tile>().direction = newDir;

        tileManager.spawnedTiles.Add(obj.GetComponent<Tile>());

        GameObject decorative = null;
        switch (direction)
        {
            case DIRECTION.FORWARD:
                var xd = new Vector3(1,0,1);
                xd= transform.forward*43.3f + transform.right *75f;
                Debug.Log(xd* 2);
                var off = new Vector3(75f, 0, 43.3f);
                //off = Quaternion.Euler(transform.rotation.x,-transform.rotation.y,transform.rotation.z) * off;
                // xd.Scale(off);
                //Decorative Pieces
                int chosenPiece = 1;
                if (chosenTile.name == "HexagonTileCrossing")
                    chosenPiece = 0;
                else
                    chosenPiece = Random.Range(1, tileManager.DecorativePrefabs.Count); 

                decorative = Instantiate(tileManager.DecorativePrefabs[chosenPiece],
                    transform.position + xd ,
                    transform.rotation,
                    transform.parent);
                decorative.GetComponent<Tile>().spawnTracks = false;

                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                xd = transform.forward * 43.3f + transform.right * -75f;
                
                decorative = Instantiate(tileManager.DecorativePrefabs[chosenPiece],
                    transform.position + xd,
                    transform.rotation,
                    transform.parent);
                decorative.GetComponent<Tile>().spawnTracks = false;
                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                
                
                break;
            case DIRECTION.RIGHT:
                break;
            case DIRECTION.LEFT:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (newDir)
        {
            case DIRECTION.FORWARD:
                break;
            case DIRECTION.RIGHT:
                decorative = Instantiate(tileManager.DecorativePrefabs[Random.Range(1, tileManager.DecorativePrefabs.Count)],
                    obj.transform.position + obj.transform.forward * 86.6f,
                    obj.transform.rotation,
                    obj.transform.parent);

                decorative.GetComponent<Tile>().spawnTracks = false;
                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                var frr = obj.transform.forward * 43.3f + obj.transform.right * -75f;

                decorative = Instantiate(tileManager.DecorativePrefabs[Random.Range(1, tileManager.DecorativePrefabs.Count)],
                    obj.transform.position + frr,
                    obj.transform.rotation,
                    obj.transform.parent);
                decorative.GetComponent<Tile>().spawnTracks = false;
                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                break;
            case DIRECTION.LEFT:
                decorative = Instantiate(tileManager.DecorativePrefabs[Random.Range(1, tileManager.DecorativePrefabs.Count)],
                    obj.transform.position + obj.transform.forward * 86.6f,
                    obj.transform.rotation,
                    obj.transform.parent);

                decorative.GetComponent<Tile>().spawnTracks = false;
                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                var fr = obj.transform.forward * 43.3f + obj.transform.right * 75f;

                decorative = Instantiate(tileManager.DecorativePrefabs[Random.Range(1, tileManager.DecorativePrefabs.Count)],
                    obj.transform.position + fr,
                    obj.transform.rotation,
                    obj.transform.parent);
                decorative.GetComponent<Tile>().spawnTracks = false;
                tileManager.spawnedTiles.Add(decorative.GetComponent<Tile>());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (PreviousDirection != DIRECTION.FORWARD)
        {
            TileManager.instance.removeTiles();
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