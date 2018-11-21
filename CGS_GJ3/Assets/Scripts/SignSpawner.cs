using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignSpawner : MonoBehaviour {

	Renderer renderSign;
	//ParticleSystem flash;
	// Use this for initialization
	void Start()
	{
		renderSign = GetComponentInChildren<Renderer>();
		renderSign.enabled = false;
		//flash = GetComponentInChildren<ParticleSystem>();
		//flash.Stop();
	}
	// Update is called once per frame
	void Update()
	{
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SpawnTrigger")
		{
			GetComponentInChildren<Animator>().SetBool("SpawnTriggered", true);
			//flash.Play();
			renderSign.enabled = true;
		}
	}
}