using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class dynamite : MonoBehaviour {

    private bool lit = false;
    private float fuse_timer = 0.0f;
    private float fuse_spark_speed = 0.0f;

    [SerializeField] float fuse_length = 5.0f;

    [SerializeField] ParticleSystem explosion_prefab;
    [SerializeField] ParticleSystem fire_prefab;
    [SerializeField] ParticleSystem sparks;
    [SerializeField] VRTK_InteractableObject linkedObject;
    [SerializeField] Transform fuse_start_pos;
    [SerializeField] Transform fuse_end_pos;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (linkedObject.IsGrabbed() && !lit)
        {
            LightFuse();
        }

		if (lit)
        {
            fuse_timer += Time.deltaTime;

            fuse_spark_speed += Time.deltaTime / fuse_length;

            sparks.gameObject.transform.position = Vector3.Lerp(fuse_start_pos.position, fuse_end_pos.position, fuse_spark_speed);

            if (fuse_timer > fuse_length)
            {
                Explode();
            }
        }
	}

    public void LightFuse()
    {
        lit = true;
        sparks.Play();
    }

    private void Explode()
    {
        Instantiate(explosion_prefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
