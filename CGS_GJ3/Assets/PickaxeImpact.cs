using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeImpact : MonoBehaviour {

    [SerializeField] ParticleSystem impact_sparks_prefab;
    [SerializeField] Transform impact_end_pos;

    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        Vector3 pos = col.contacts[0].point;

        if (Vector3.Distance(pos, impact_end_pos.position) < 0.5f)
        {
            if (col.impulse.magnitude > 10.0f)
            {
                SoundManager.instance.PlaySingleAtSource(audioSource);

                ParticleSystem obj = Instantiate(impact_sparks_prefab, pos, transform.rotation);

                Destroy(obj.gameObject, 1.0f);
            }
        }
    }
}
