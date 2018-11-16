using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Rigidbody movingObject;
	public Vector3 movingSpeed = new Vector3(3,0,0);

	// Use this for initialization
	void Start () {
		movingObject = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		movingObject.velocity = movingSpeed;
	}
}
