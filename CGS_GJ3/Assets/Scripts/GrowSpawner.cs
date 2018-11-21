using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowSpawner : MonoBehaviour {

	Transform objectToGrow;
	Renderer[] treeRend;
	Renderer renderTree;
	ParticleSystem leaves;
	// Use this for initialization
	void Start()
	{
		treeRend = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderTree in treeRend)
		{
			renderTree.enabled = false;
		}
		leaves = GetComponentInChildren<ParticleSystem>();
		leaves.Stop();
	}

	// Update is called once per frame
	void Update()
	{
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SpawnTrigger")
		{
			foreach (Renderer renderTree in treeRend)
			{
				renderTree.enabled = true;
			}
			GetComponentInChildren<Animator>().SetBool("GrowTriggered", true);
			leaves.Play();
		}
	}
}