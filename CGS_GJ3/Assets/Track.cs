using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {
    public Pixelplacement.Spline spline;
    public Track forwardTrack;
    public Track backwardTrack;
    public Transform ForwardDetection;
    public Transform BackwardDetection;
    // Use this for initialization
    void Start () {
        
        if(ForwardDetection==null)
        {
            ForwardDetection = transform;
        }
        if(BackwardDetection==null)
        {
            BackwardDetection = transform;
        }
        if(spline==null)
        {
            spline = GetComponent<Pixelplacement.Spline>();

        }
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");
        //layerMask = ~layerMask;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(ForwardDetection.position, ForwardDetection.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ForwardDetection.position, ForwardDetection.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if(forwardTrack==null)
            forwardTrack = hit.collider.gameObject.GetComponent<Track>();
        }
        if (Physics.Raycast(BackwardDetection.position, -ForwardDetection.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(BackwardDetection.position, BackwardDetection.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            if (backwardTrack == null)
                backwardTrack = hit.collider.gameObject.GetComponent<Track>();
        }
    }
	


	// Update is called once per frame
	void Update () {
		
	}

   
}
