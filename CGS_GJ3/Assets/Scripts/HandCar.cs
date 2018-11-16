using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCar : MonoBehaviour
{

    enum PumpState
    {
        IDLE = 0,
        UP = 1,
        DOWN = 2
    }

    [SerializeField] private float speedModifier =5;
    [SerializeField] private Transform movementLever;
    [SerializeField]
    private List<WheelCollider> motorWheels;
    private bool coroutineStarted = false;
    private int modifier = 20;
    private PumpState pumpState = PumpState.IDLE;
    private float timer = 0.0f;
    private Rigidbody myRigidbody;
    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = Vector3.down;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(myRigidbody.worldCenterOfMass, 0.01f);
    }

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
                    modifier += 10;
                    coroutineStarted = false;

                    yield break;
                }
            }
            else
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    Debug.Log("nice one!");
                    modifier += 10;
                    coroutineStarted = false;
                    yield break;
                }

            }
            timer += Time.unscaledDeltaTime;
            yield return null;

        }

        modifier = 10;
    }

    private void AddForce(float force)
    {
        Debug.Log(force * speedModifier);
        //GetComponent<Rigidbody>().AddForce(transform.forward * force * speedModifier);
        foreach (var wheel in motorWheels)
        {
            wheel.motorTorque = force * speedModifier;
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
                movementLever.Rotate(Vector3.right, force);
                AddForce(Mathf.Abs(force));
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
            Debug.Log("GO DOWN");
        }
        if (movementLever.localEulerAngles.x < 350 && movementLever.localEulerAngles.x > 180)
        {
            if(movementLever.localEulerAngles.x < 345)
                movementLever.localRotation = Quaternion.Euler(345.01f, 0, 0);

            if (!coroutineStarted && pumpState != PumpState.DOWN)
                StartCoroutine(WaitForInput(true));

            //TODO: UI
            Debug.Log("GO UP");
            //movementLever.localRotation.SetEulerAngles(14.99f, 0, 0);
        }
        //if (movementLever.localEulerAngles.x < 165)
        //{
        //    movementLever.localRotation.SetEulerAngles(165, 0, 0);

        //}
    }
}
