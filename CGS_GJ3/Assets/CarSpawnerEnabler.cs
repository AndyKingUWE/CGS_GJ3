using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerEnabler : MonoBehaviour {
    public bool enableLeft;
    public CarSpawner left;
    public CarSpawner right;
	// Use this for initialization
	void Start () {
		if(enableLeft)
        {
            left.gameObject.SetActive(true);

            right.gameObject.SetActive(false);
        }
        else
        {
            right.gameObject.SetActive(true);

            left.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
