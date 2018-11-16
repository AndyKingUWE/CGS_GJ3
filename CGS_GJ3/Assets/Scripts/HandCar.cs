using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class HandCar : MonoBehaviour
{

    enum PumpState
    {
        IDLE = 0,
        UP = 1,
        DOWN = 2
    }
    [SerializeField]
    private Track currentTrack;
    [SerializeField] private float speedModifier =5;
    [SerializeField] private Transform movementLever;
    [SerializeField]
    private List<WheelCollider> motorWheels;
    [SerializeField] private List<BoxCollider> triggers;
    private bool coroutineStarted = false;
    private float modifier = 20;
    private PumpState pumpState = PumpState.IDLE;
    private float timer = 0.0f;
    private Rigidbody myRigidbody;
    [SerializeField] private Transform axle;
    [SerializeField] private Transform axle2;
    private float ClosestPoint;
    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = Vector3.down;
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (other.collider.tag == "Tracks")
        //{
        //    myRigidbody.AddForce(-other.relativeVelocity);
        //    Debug.Log(other.relativeVelocity);
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Tracks");
        //layerMask = ~layerMask;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + transform.up*10, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position + transform.up * 10, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            var prevTrack = currentTrack;
        
            currentTrack = hit.collider.gameObject.GetComponent<Track>();
            if(currentTrack!=prevTrack)
            {
                ResetPosition();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            speedModifier++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            speedModifier--;
        }
        foreach (var wheel in motorWheels)
        {
            //wheel.motorTorque = speedModifier;
        }
        MoveLever();
        if(Input.GetKeyDown(KeyCode.U))
        {
            AddForce(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Brake(1);
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            Brake(0);

        }
        if(modifier>20)
        {
            AddForce(modifier * Time.unscaledDeltaTime);
        }
    }

    private void LateUpdate()
    {
        //ResetPosition();
        //if(myRigidbody.velocity.magnitude<3)
        //{
        //    myRigidbody.
        //}
    }

    internal void SetPosition()
    {
        var newPos = currentTrack.spline.GetPosition(ClosestPoint, true, 100);
        newPos.y = transform.position.y;
        myRigidbody.MovePosition(newPos);
        myRigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
    }

    internal void ResetPosition()
    {
        ClosestPoint = currentTrack.spline.ClosestPoint(transform.position);
        var newPos = transform.position;
        newPos.x = currentTrack.spline.GetPosition(ClosestPoint,true,10000).x;
        newPos.z = currentTrack.spline.GetPosition(ClosestPoint, true, 10000).z;
        myRigidbody.MovePosition(newPos);
        myRigidbody.MoveRotation(Quaternion.Euler(0, 0, 0));
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(myRigidbody.worldCenterOfMass, 0.01f);
    //}

    private IEnumerator WaitForInput(bool positive)
    {
        if (positive)
        {

            pumpState = PumpState.DOWN;
        }
        else
        {
            pumpState = PumpState.UP;
        }
        coroutineStarted = true;
        var timer = 0.0f;
        while (timer < 1.0f)
        {
            if (positive)
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    Debug.Log("nice one!");
                    modifier ++;
                    coroutineStarted = false;
                    if(modifier>100)
                    {
                        modifier = 100;
                    }

                    yield break;
                }
            }
            else
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    Debug.Log("nice one!");
                    modifier ++;
                    if (modifier > 100)
                    {
                        modifier = 100;
                    }
                    coroutineStarted = false;
                    yield break;
                }

            }
            timer += Time.unscaledDeltaTime;
            yield return null;

        }

        modifier = 20;
    }

    private void Brake(float force)
    {
        foreach (var wheel in motorWheels)
        {
            wheel.brakeTorque = force * speedModifier;
        }
    }

    private void AddForce(float force)
    {
        ClosestPoint +=  Time.unscaledDeltaTime;
        if(ClosestPoint>1)
        {
            ClosestPoint = 1 - ClosestPoint;
            currentTrack = currentTrack.forwardTrack;
        }
        SetPosition();
        Debug.Log(force * speedModifier);
        //GetComponent<Rigidbody>().AddForce(transform.forward * force * speedModifier);
        //axle.Rotate(Vector3.right * force* speedModifier);
        //axle2.Rotate(Vector3.right * force* speedModifier);
        foreach (var wheel in motorWheels)
        {
            //wheel.motorTorque = force * speedModifier;
        }
    }

    private void MoveLever()
    {
        if (movementLever.localEulerAngles.x >= 345 || movementLever.localEulerAngles.x <= 15)
        {
            if (Input.GetAxis("Vertical") == 0)
            {
                timer += Time.unscaledDeltaTime;
            }
            else
            {
                var force = Input.GetAxis("Vertical") * Time.unscaledDeltaTime * modifier;
                modifier += Mathf.Abs( force);
                movementLever.Rotate(Vector3.right, force);
                timer = 0.0f;
            }
            if (timer >= 1.0f)
            {

                pumpState = PumpState.IDLE;
            }
        }


        if (movementLever.localEulerAngles.x > 10 && movementLever.localEulerAngles.x < 180)
        {
            if (!coroutineStarted && pumpState != PumpState.UP)
                StartCoroutine(WaitForInput(false));

            if (movementLever.localEulerAngles.x > 15)
                movementLever.localRotation = Quaternion.Euler(14.99f, 0, 0);
            //TODO: UI
            //Debug.Log("GO DOWN");
        }
        if (movementLever.localEulerAngles.x < 350 && movementLever.localEulerAngles.x > 180)
        {
            if(movementLever.localEulerAngles.x < 345)
                movementLever.localRotation = Quaternion.Euler(345.01f, 0, 0);

            if (!coroutineStarted && pumpState != PumpState.DOWN)
                StartCoroutine(WaitForInput(true));

            //TODO: UI
            //Debug.Log("GO UP");
            //movementLever.localRotation.SetEulerAngles(14.99f, 0, 0);
        }
        //if (movementLever.localEulerAngles.x < 165)
        //{
        //    movementLever.localRotation.SetEulerAngles(165, 0, 0);

        //}
    }
}
