using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_sound : MonoBehaviour
{
    public AudioSource AS;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pickaxe")
        {
            AS.Play();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
