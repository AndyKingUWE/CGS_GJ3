using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    public List<GameObject> cars;
    public float delay = 2f;
    private float timer = 0.0f;
	// Use this for initialization
	void Start () {
        SpawnCar();

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer>delay)
        {
            SpawnCar();
        }
    }

    private void SpawnCar()
    {
        timer = 0.0f;
        delay = UnityEngine.Random.Range(4f, 7f);
        Instantiate(cars[UnityEngine.Random.Range(0, cars.Count)], transform.position, transform.rotation);
    }
}
