using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {
    [SerializeField] private HandCar handCar;
    Vector3 previousPosition= Vector3.zero;
    [SerializeField] private List<WheelCollider> wheels;
    // Use this for initialization
    public float dif;
    Vector3 lastHit;
    Vector3 lastHitob;

	void Start () {
		
	}

    private void FixedUpdate()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");
        //layerMask = ~layerMask;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward*4 + Vector3.down + Vector3.right* 0.05f ), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 4 + Vector3.down + Vector3.right * 0.05f) * hit.distance, Color.yellow);
            var xd = hit.point- hit.collider.transform.position;
            //Debug.Log("H: " +  + ",O: " + transform.InverseTransformPoint(hit.collider.transform.position).y);
             dif = transform.InverseTransformPoint(hit.point).y - transform.InverseTransformPoint(hit.collider.transform.position).y ;
            lastHit = hit.point;
            lastHitob = hit.collider.transform.position;
            //Debug.Log(transform.InverseTransformPoint(hit.point).y - transform.InverseTransformPoint(hit.collider.transform.position).y);
            //previousPosition = transform.InverseTransformPoint(hit.point);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward *4 + Vector3.down + Vector3.right * 0.05f) * 1000, Color.white);
            //handCar.ResetPosition();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
