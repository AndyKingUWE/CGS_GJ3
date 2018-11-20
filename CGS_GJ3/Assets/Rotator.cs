﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [SerializeField] private HandCar hc;
    Transform tr;
    public float prevangle = 0.0f;
    public float curangle = 0.0f;
	// Use this for initialization
	void Start () {
        tr = transform.parent;
        prevangle = tr.localEulerAngles.x;
        if (prevangle>180)
        {
            prevangle -= 360;
        }
    }
	
	// Update is called once per frame
	void Update () {
        curangle = tr.localEulerAngles.x;
        if (curangle > 180)
        {
            curangle -= 360;
        }
            if(curangle>prevangle)
            {
            hc.vrinput = 1.0f;
            }
            else if (curangle < prevangle)
        {
            hc.vrinput = -1.0f;
        }
            else
        {
            hc.vrinput = 0.0f;

        }
	}

    private void LateUpdate()
    {
        prevangle = curangle;
    }
}
