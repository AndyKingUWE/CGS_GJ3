using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RockyMountainSpawner : MonoBehaviour
{

    public List<GameObject> boulderGameObjects;
    public GameObject playerTrigger;                    //Will need to be changed for the main scene later
    //private BoxCollider boxC;

    public int numOfBoulders = 10;                      //Amount of boulders to spawn
    public float distanceFromGround = 15f;              //Distance from the ground when boulders spawn
    public float distanceFromPlayer = 0.5f;             //Scale 0 - 1 of how far from the player ex. 0.5 is halfway
    public float minimumDistanceOffset = 10f;            //Minimum distance the boulder will spawn from player
    public float maximumDistanceOffset = 20f;            //Maximum distance the boulder will spawn from player

    private Vector3 positionGB;                           //Just setting the position of the mountain

    void Awake()
    {
        //boxC = GetComponent<BoxCollider>();
        positionGB = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

	// Use this for initialization
	void Start ()
	{

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")       //Check for right object, don't know if tag is best system
        {
            //Vector distance between mountain and player
            Vector3 distance = new Vector3((positionGB.x - other.transform.position.x)* distanceFromPlayer,
                positionGB.y - other.transform.position.y + distanceFromGround,
                (positionGB.z - other.transform.position.z) * distanceFromPlayer);
            Vector3 offset = new Vector3(0f, 0f, Random.Range(minimumDistanceOffset, maximumDistanceOffset));

            for (int i = 0; i < numOfBoulders; i++)
            {
                offset = new Vector3(0f, 0f, Random.Range(minimumDistanceOffset, maximumDistanceOffset));
                var bBoulder = Instantiate(boulderGameObjects[Random.Range(0, boulderGameObjects.Count)],
                    distance + offset,
                    Quaternion.identity);
                Destroy(bBoulder, 6f);
            }
        }
    }
}
