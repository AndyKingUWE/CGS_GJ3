using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeImpact : MonoBehaviour {

    [SerializeField] ParticleSystem impact_sparks_prefab;
    [SerializeField] Transform impact_end_pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        Vector3 pos = col.contacts[0].point;

        Debug.Log(Vector3.Distance(pos, impact_end_pos.position));

        if (Vector3.Distance(pos, impact_end_pos.position) < 0.5f)
        {
            if (col.impulse.magnitude > 10.0f)
            {
                ParticleSystem obj = Instantiate(impact_sparks_prefab, pos, transform.rotation);

                Destroy(obj, 1.0f);
            }
        }
    }
}
