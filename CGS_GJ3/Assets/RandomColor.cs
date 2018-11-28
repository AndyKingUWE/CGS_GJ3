using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour {

    // Use this for initialization
    public int materialId = 0;
	void Start ()
    {
        var meshrenderer =
        GetComponent<MeshRenderer>();
        var materials = meshrenderer.materials;
        var number = Random.Range(0, WeatherManager.instance.autumnMaterials.Count);
        materials[materialId] = WeatherManager.instance.autumnMaterials[number];
        meshrenderer.materials = materials;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
