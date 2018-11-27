using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpawner : MonoBehaviour {

	//Transform childToFall;
	Vector3 startPos;
	Vector3 endPos;
    Quaternion startRot;
    Quaternion endRot; 
    Tile tile; 
    float fallDistance, startTime;
    public float delay;
    Renderer rend; 
    // Use this for initialization
    void Start()
	{
        if (delay != 0)
        {
            tile = transform.GetComponentInParent<Tile>();
            if (tile == null)
                Debug.Log("FallSpawner cannot find ProceduralRail in tile"); 

            startPos = transform.position + Vector3.up * TileManager.instance.heightUp;
            startRot = Quaternion.Euler(-90, 0, 0);
           
            endRot = transform.rotation;
            endPos = transform.position;
            transform.position = startPos;
            transform.rotation = startRot; 
            fallDistance = Vector3.Distance(startPos, endPos);
            startTime = Time.time;
            rend = gameObject.GetComponent<Renderer>();
            rend.enabled = false; 
        }
        else
            enabled = false; 
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time - startTime > delay)
		{
            rend.enabled = true; 
            float t = (Time.time - (startTime + delay)) / TileManager.instance.trackLaySpeed;
            //t = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t); 
            //Stop lerp sin waving back up
            if (transform.position.y < endPos.y + 0.01f)
            {
                //transform.position = endPos;
                //SoundManager.instance.PlaySingle("Crash_hitWoodCrate");
                enabled = false; 
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

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.tag == "SpawnTrigger")
	//		triggered = true;
	//}
}