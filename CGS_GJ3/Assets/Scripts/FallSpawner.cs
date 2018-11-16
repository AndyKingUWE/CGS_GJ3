using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpawner : MonoBehaviour {

	Transform childToFall;
	Transform parentLocation;
	public Vector3 startPos;
	public Vector3 endPos;
	public float fallDistance;
	bool triggered = false;

	// Use this for initialization
	void Start()
	{
		parentLocation = transform;
		childToFall = transform.GetChild(0);
		startPos = new Vector3(parentLocation.localPosition.x, parentLocation.localPosition.y + 20, parentLocation.localPosition.z);
		childToFall.transform.position = startPos;
		endPos = parentLocation.position;

	}

	// Update is called once per frame
	void Update()
	{
		if (triggered)
		{
			childToFall.transform.position = Vector3.Lerp(startPos, endPos, fallDistance);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SpawnTrigger")
			triggered = true;
	}
}