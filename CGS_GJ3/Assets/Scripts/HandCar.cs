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
        DOWN = 2,
        GRABBED = 3
    }
    public float slowdownSpeed = 2.0f;
    [SerializeField]
    private Track currentTrack;
    [SerializeField] private float speedModifier = 5;
    [SerializeField] private Transform movementLever;
    [SerializeField] private Transform brakeLever;
    [SerializeField]
    private List<WheelCollider> motorWheels;
    [SerializeField] private List<BoxCollider> triggers;
    private bool coroutineStarted = false;
    private float modifier = 1;
    [SerializeField] private PumpState pumpState = PumpState.IDLE;
    private float timer = 0.0f;
    private Rigidbody myRigidbody;
    [SerializeField] private Transform axle;
    [SerializeField] private Transform axle2;
    private float ClosestPoint;
    public float input = 0.0f;
    public float vrinput = 0.0f;
    public float speed;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        GetCurrentTrack();
        BrakeLever();
        MoveLever();        

        #region debug
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            AddForce(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Brake(1);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Brake(0);

        }
        #endregion



        //if (modifier > 20)
        //{
        //    AddForce(modifier * Time.unscaledDeltaTime);
        //}

        ClosestPoint = currentTrack.spline.ClosestPoint(transform.position);

        if (ClosestPoint > 1)
        {
            ClosestPoint = 1 - ClosestPoint;
            currentTrack = currentTrack.forwardTrack;
        }

        transform.position += transform.forward * Time.fixedDeltaTime * input * speedModifier;


        var newforward = currentTrack.spline.Forward(ClosestPoint);
        transform.forward = newforward;
        

        if (pumpState==PumpState.IDLE)
            input += (0-input)* (slowdownSpeed/100f);
        if (input < 0.0f)
            input = 0.0f;

        var start = currentTrack.spline.GetPosition(ClosestPoint);
        start.y = 0;
        var end = transform.position;
        end.y = 0;
        var distance = Vector3.Distance(start, end);
        //Debug.Log(distance);
        if (distance >= 0.1f)
        {
            end.x = start.x;
            end.z = start.z;
            end.y = transform.position.y;

            //transform.position = end;
        }
        speed = myRigidbody.velocity.magnitude;
    }

    private void Update()
    {
        float spd = input * 3.14f;
        axle.Rotate(Vector3.right, spd);
        axle2.Rotate(Vector3.right, spd);
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
        newPos.x = currentTrack.spline.GetPosition(ClosestPoint, true, 1000).x;
        newPos.z = currentTrack.spline.GetPosition(ClosestPoint, true, 1000).z;
        transform.position = newPos;
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
                if (vrinput > 0)
                {
                    Debug.Log("nice one!");
                    modifier++;
                    coroutineStarted = false;
                    if (modifier > 100)
                    {
                        modifier = 100;
                    }

                    yield break;
                }
            }
            else
            {
                if (vrinput < 0)
                {
                    Debug.Log("nice one!");
                    modifier++;
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
        coroutineStarted = false;
    }

    private void Brake(float force)
    {
        //foreach (var wheel in motorWheels)
        //{
        //    wheel.brakeTorque = force * speedModifier;
        //}

        float dampeningForce = 0.001f;

        input -= (force * dampeningForce);

        if (input < 0)
        {
            input = 0;
        }
    }

    private void AddForce(float force)
    {
        ClosestPoint += Time.unscaledDeltaTime;
        if (ClosestPoint > 1)
        {
            ClosestPoint = 1 - ClosestPoint;
            currentTrack = currentTrack.forwardTrack;
        }
        //SetPosition();
        //GetComponent<Rigidbody>().AddForce(transform.forward * force * speedModifier);
        //axle.Rotate(Vector3.right * force* speedModifier);
        //axle2.Rotate(Vector3.right * force* speedModifier);
        foreach (var wheel in motorWheels)
        {
            wheel.motorTorque = force * speedModifier;
        }
    }

    private void BrakeLever()
    {
        float force = brakeLever.localEulerAngles.x;
        //check if brake lever is pulled
        if ((360 - force) < 20)
        {
            force = (360 - force) / 10;
            force = force * force;

            Debug.Log(force);
            Brake(force);
        }
    }

    private void MoveLever()
    {
        
        //within movement range
        if (movementLever.parent.localEulerAngles.x >= 330 || movementLever.parent.localEulerAngles.x <= 30)
        {
            if (vrinput == 0)
            {
                timer += Time.unscaledDeltaTime;
            }
            else if (!coroutineStarted && vrinput != 0)
            {
                if(pumpState==PumpState.IDLE)
                {
                    pumpState = PumpState.GRABBED;
                }
                var force = vrinput * Time.fixedDeltaTime * modifier;
                modifier += Mathf.Abs(force);
                //movementLever.Rotate(Vector3.right, force);
                timer = 0.0f;
                input += Mathf.Abs(force / modifier);
            }
            if (timer >= 1.0f)
            {
                pumpState = PumpState.IDLE;
            }
        }


        if (movementLever.parent.localEulerAngles.x > 20 && movementLever.parent.localEulerAngles.x < 180)
        {
            if (!coroutineStarted && pumpState != PumpState.UP)
                StartCoroutine(WaitForInput(false));

            //if (movementLever.localEulerAngles.x > 15)
            //    movementLever.localRotation = Quaternion.Euler(14.99f, 0, 0);
            //TODO: UI
            //Debug.Log("GO DOWN");
        }
        if (movementLever.parent.localEulerAngles.x < 340 && movementLever.parent.localEulerAngles.x > 180)
        {
            //if (movementLever.localEulerAngles.x < 345)
            //    movementLever.localRotation = Quaternion.Euler(345.01f, 0, 0);

            if (!coroutineStarted && pumpState != PumpState.DOWN)
                StartCoroutine(WaitForInput(true));

            //TODO: UI
            //Debug.Log("GO UP");
            //movementLever.localRotation.SetEulerAngles(14.99f, 0, 0);
        }
        
    }
}
