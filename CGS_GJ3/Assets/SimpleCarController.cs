using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public Transform leftWheelTransform;
    public WheelCollider rightWheel;
    public Transform rightWheelTransform;
    public bool motor;
    public bool steering;
}

public class SimpleCarController : MonoBehaviour
{
    public enum CarState
    {
        WAIT = 0,
        DRIVE = 1,
        BRAKE = 2
    }
    public CarState carState = CarState.WAIT;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float speed;
    public float brakeForce;
    private float distance;
    private Rigidbody rigidbody;
    private float timer;
    private float lifetime = 30;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Brake(float force)
    {
        carState = CarState.BRAKE;
        distance = force;
    }

    public void Drive()
    {
        carState = CarState.DRIVE;
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider , Transform transform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        transform.position = position;
        transform.rotation = rotation;
    }
    

    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > lifetime)
            Destroy(gameObject);
           speed = Vector3.Dot(rigidbody.velocity, transform.forward);
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor &&carState== CarState.DRIVE)
            {
                axleInfo.leftWheel.motorTorque = maxMotorTorque;
                axleInfo.rightWheel.motorTorque = maxMotorTorque;
            }
            if (carState == CarState.BRAKE)
            {
                axleInfo.leftWheel.brakeTorque = distance * brakeForce;
                axleInfo.rightWheel.brakeTorque = distance * brakeForce;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftWheelTransform);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightWheelTransform);
        }
    }
}