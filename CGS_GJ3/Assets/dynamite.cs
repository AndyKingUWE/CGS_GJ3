using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamite : MonoBehaviour {

    private bool lit = false;
    private float fuse_timer = 0.0f;

    [SerializeField] float fuse_length = 5.0f;

    [SerializeField] ParticleSystem explosion_prefab;
    [SerializeField] ParticleSystem fire_prefab;
    [SerializeField] ParticleSystem sparks;
    //[SerializeField] VRTK_InteractableObject linkedObject;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (lit)
        {
            fuse_timer += Time.deltaTime;

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
        Instantiate(fire_prefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    //protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    //{
    //    FireProjectile();
    //}
}
