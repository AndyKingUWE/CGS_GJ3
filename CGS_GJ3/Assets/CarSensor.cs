using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSensor : MonoBehaviour {

    private SimpleCarController carController;
    private List<RaycastHit> collisions;
    private bool brake = false;
    // Use this for initialization
    void Start () {
        carController = GetComponentInParent<SimpleCarController>();
	}
	
	// Update is called once per frame
	void Update () {
        brake = false;
        collisions = Physics.CapsuleCastAll(transform.position, transform.position + transform.forward * carController.speed, 1.5f, transform.forward, 0).ToList();
        if (collisions != null)
        {
            foreach (var item in collisions)
            {
                if (item.collider.gameObject.CompareTag("Destroyable"))
                {
                    carController.Brake(Vector3.Distance(transform.position, item.collider.transform.position));
                    brake = true;
                }
            }
        }
        
        if(!brake)
        {
            carController.Drive();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * carController.speed);
        if (collisions == null)
            return;
        foreach (var item in collisions)
        {
            if (item.collider.gameObject.CompareTag("Destroyable"))
            {
                Gizmos.DrawSphere(item.transform.position, 1);
            }
        }
    }

}
