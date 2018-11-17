using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{

    [SerializeField]
    private Track currentTrack;
    private float ClosestPoint;
    private float speed = 0.0f;
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private bool backward = false;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(TrainBehaviour());
    }

    IEnumerator TrainBehaviour()
    {
        while (true)
        {
            GetCurrentTrack();
            ClosestPoint = currentTrack.spline.ClosestPoint(transform.position);
            if (ClosestPoint > 1)
            {
                ClosestPoint = 1 - ClosestPoint;
                if (backward)
                    currentTrack = currentTrack.backwardTrack;
                else
                    currentTrack = currentTrack.forwardTrack;

            }

            var newforward = currentTrack.spline.Forward(ClosestPoint);
            if (newforward != Vector3.zero && newforward != transform.forward)
            {
                if (backward)
                {
                    newforward = -newforward;
                    if (currentTrack.spline.direction == Pixelplacement.SplineDirection.Backwards)
                    {
                        newforward = -newforward;
                    }

                }
                else
                {

                    transform.forward = newforward;
                    if (currentTrack.spline.direction == Pixelplacement.SplineDirection.Backwards)
                    {
                        newforward = -newforward;
                    }
                }
                transform.forward = newforward;
            }
            if (speed < maxSpeed)
                speed += Time.fixedDeltaTime;
            else if (speed > maxSpeed)
                speed = maxSpeed;
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
       
    }

    void GetCurrentTrack()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");

        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.up * 5, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            var prevTrack = currentTrack;

            currentTrack = hit.collider.gameObject.GetComponent<Track>();

            if (currentTrack != prevTrack)
            {
                ResetPosition();
            }
        }
    }

    internal void ResetPosition()
    {
        ClosestPoint = currentTrack.spline.ClosestPoint(transform.position);
        var newPos = transform.position;
        newPos.x = currentTrack.spline.GetPosition(ClosestPoint, true, 1000).x;
        newPos.z = currentTrack.spline.GetPosition(ClosestPoint, true, 1000).z;
        transform.position = newPos;
    }


    void FixedUpdate()
    {

        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
