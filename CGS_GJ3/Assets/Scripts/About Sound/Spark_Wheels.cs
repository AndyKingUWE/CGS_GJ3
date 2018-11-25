using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Wheels : MonoBehaviour {
    public AudioSource AS;

    public float speed;
    private bool chang = true;
    private void handcarVoiceControl()
    {
        AS.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
