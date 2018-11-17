using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpawner : MonoBehaviour {

	//Transform childToFall;
	public Vector3 startPos;
	public Vector3 endPos;
    public float speed; 
	float fallDistance; 
	bool triggered = true;

	// Use this for initialization
	void Start()
	{
        //childToFall = transform.GetChild(0);
        endPos = transform.position;
        startPos = new Vector3(transform.localPosition.x, transform.localPosition.y + 20, transform.localPosition.z);
		transform.position = startPos;
        fallDistance = Vector3.Distance(startPos, endPos); 
	}

	// Update is called once per frame
	void Update()
	{
		if (triggered)
		{
            //Distance moved = time * speed 
            float distCovered = Time.time * speed;
            //Fraction of journey completed
            float fracJourney = distCovered / fallDistance; 
            //Lerp by fraction of distance covered
			transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SpawnTrigger")
			triggered = true;
	}
}