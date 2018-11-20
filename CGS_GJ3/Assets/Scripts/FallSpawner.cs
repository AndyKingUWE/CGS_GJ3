using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpawner : MonoBehaviour {

	//Transform childToFall;
	Vector3 startPos;
	Vector3 endPos;
    ProceduralRail tile; 
    float fallDistance, startTime;
    bool triggered = true;

    // Use this for initialization
    void Start()
	{
        tile = transform.parent.parent.GetComponent<ProceduralRail>(); 
        startPos = transform.position + transform.up * tile.origin.heightUp;
        endPos = transform.position;
        transform.position = startPos;
        fallDistance = Vector3.Distance(startPos, endPos);
        startTime = Time.time; 
	}

	// Update is called once per frame
	void Update()
	{
		if (triggered)
		{
            float t = (Time.time - startTime) / tile.origin.speed;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            //Stop lerp sin waving back up
            if (transform.position.y < endPos.y + 0.01f)
            {
                triggered = false;
                transform.position = endPos; 
            }
                
        }
	}

    //Vector3 CustomLerp()
    //{

    //      if ((Input.GetKey("left")) && (Speed < MaxSpeed))
    //            Speed = Speed - Acceleration  Time.deltaTime;

    //    else if ((Input.GetKey("right")) && (Speed > -MaxSpeed))
    //            Speed = Speed + Acceleration  Time.deltaTime;
    //    else
    //    {
    //                    if (Speed > Deceleration  Time.deltaTime)
    //            Speed = Speed - Deceleration  Time.deltaTime;
    //        else if (Speed < -Deceleration  Time.deltaTime)
    //            Speed = Speed + Deceleration  Time.deltaTime;
    //        else
    //            Speed = 0;
    //        }


    //        transform.position.x = transform.position.x + Speed* Time.deltaTime;
    //    }
    //}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SpawnTrigger")
			triggered = true;
	}
}