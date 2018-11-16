using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {
    public Pixelplacement.Spline spline;
    public Track forwardTrack;
    public Track backwardTrack;
    // Use this for initialization
    void Start () {
        spline = GetComponent<Pixelplacement.Spline>();
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");
        //layerMask = ~layerMask;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward ), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            forwardTrack = hit.collider.gameObject.GetComponent<Track>();
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            backwardTrack = hit.collider.gameObject.GetComponent<Track>();
        }
    }
	


	// Update is called once per frame
	void Update () {
		
	}

   
}
