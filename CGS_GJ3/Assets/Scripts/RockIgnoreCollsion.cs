using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockIgnoreCollsion : MonoBehaviour
{
    private SphereCollider sphereC;

    // Use this for initialization
    void Start()
    {
        sphereC = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<SphereCollider>());
            this.GetComponent<Rigidbody>().Sleep();;
        }
    }
}
