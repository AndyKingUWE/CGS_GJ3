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
            yield return new WaitForSecondsRealtime(0.1f);
        }

    }

    void GetCurrentTrack()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");

        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.up * 5, transform.TransformDirection(Vector3.down), out hit, 10, layerMask))
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
        var pos = currentTrack.spline.GetPosition(ClosestPoint);
        newPos.x = pos.x;
        newPos.z = pos.z;
        transform.position = newPos;
    }


    void FixedUpdate()
    {


    }
    // Update is called once per frame
    void Update()
    {
        
        if (speed < maxSpeed)
            speed += Time.deltaTime;
        else if (speed > maxSpeed)
            speed = maxSpeed;
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
