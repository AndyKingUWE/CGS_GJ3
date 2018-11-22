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
    private Track currentTrack;
    [SerializeField] private Transform chassis;
    [SerializeField] private float speedModifier = 5;
    [SerializeField] private Transform movementLever;
    [SerializeField] private Transform brakeLever;
    [SerializeField] private List<Transform> wheelTransforms;
    private bool coroutineStarted = false;
    private float modifier = 1;private PumpState pumpState = PumpState.IDLE;
    private float timer = 0.0f;
    private Rigidbody myRigidbody;
    private float ClosestPoint;
    public float input = 0.0f;
    public float vrinput = 0.0f;
    public float speed;
    [SerializeField] private AudioClip track;
    private int trackCount = 0;
    private float soundtimer=0f;
    [SerializeField] private List<GameObject> brakingParticles;

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
                trackCount++;
                if(trackCount>= 1)
                {
                       var mod = input;
                    if(mod>5)
                    {
                        mod = 5;
                    }
                    mod = 0.75f / mod;
                    if(mod<0.1f)
                    {
                        mod = 0.1f;
                    }
                    if(soundtimer > 0.5f)
                    {
                        SoundManager.instance.PlaySingle(track);
                        //SoundManager.instance.PlaySingleDelayed(track, mod);
                        soundtimer = 0;
                        StartCoroutine(Shake());
                    }
                    trackCount = 0;
                }
                ResetPosition();
            }
        }
    }

    private IEnumerator Shake()
    {
        chassis.localPosition -= Vector3.up * 0.01f * UnityEngine.Random.Range(1.0f,5.0f);
        while (chassis.localPosition.y<0)
        {
            chassis.localPosition += Time.fixedDeltaTime * Vector3.up;
            yield return null;
        }
        chassis.localPosition = Vector3.zero;
        yield return null;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        soundtimer += Time.fixedDeltaTime;
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




        ClosestPoint = currentTrack.spline.ClosestPoint(transform.position);

        if (ClosestPoint > 1)
        {
            ClosestPoint = 1 - ClosestPoint;
            currentTrack = currentTrack.forwardTrack;
        }

        var position = transform.position + transform.forward * Time.fixedDeltaTime * input * speedModifier;
        myRigidbody.MovePosition(position);


        var newforward = currentTrack.spline.Forward(ClosestPoint);
        var transformtemp = transform;
        transformtemp.forward = newforward;
        myRigidbody.MoveRotation(transformtemp.rotation);
        

        if (pumpState==PumpState.IDLE)
            input += (0-input)* (slowdownSpeed/100f);
        if (input < 0.0f)
            input = 0.0f;

       
        speed = myRigidbody.velocity.magnitude;
    }

    private void Update()
    {
        foreach (var item in wheelTransforms)
        {
            item.Rotate(Vector3.right, speed);
        }
        //axle.Rotate(Vector3.right, spd);
        //axle2.Rotate(Vector3.right, spd);
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
        
    }

    private void BrakeLever()
    {
        bool disable_sparks = false;

        float spd = input * 3.14f;

        float force = brakeLever.localEulerAngles.x;
        //check if brake lever is pulled
        if ((360 - force) < 30)
        {
            force = (360 - force) / 10;
            force = force * force;                       

            if ((360 - force) > 10)
            {
                if (spd > 7.0f)
                {
                    foreach (var item in brakingParticles)
                    {
                        item.SetActive(true);
                    }
                }
            }

            Brake(force);
        }        

        else
        {
            disable_sparks = true;
        }

        if (spd < 7.0f)
        {
            disable_sparks = true;
        }

        if (disable_sparks)
        {
            foreach (var item in brakingParticles)
            {
                //item.SetActive(false);
            }
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


        if (movementLever.parent.localEulerAngles.x > 15 && movementLever.parent.localEulerAngles.x < 180)
        {
            if (!coroutineStarted && pumpState != PumpState.UP)
                StartCoroutine(WaitForInput(false));

            //if (movementLever.localEulerAngles.x > 15)
            //    movementLever.localRotation = Quaternion.Euler(14.99f, 0, 0);
            //TODO: UI
            //Debug.Log("GO DOWN");
        }
        if (movementLever.parent.localEulerAngles.x < 345 && movementLever.parent.localEulerAngles.x > 180)
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
