using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour {

    [SerializeField] private float breakForce;
    [SerializeField] private AudioClip spawnAudioClip;
    [SerializeField] private AudioClip deathAudioClip;
    [SerializeField] private ParticleSystem spawnParticleSystem;
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private LayerMask dieOnContactWith;

    // Use this for initialization
    void Start () {
        OnSpawn();
	}
	


    public virtual void OnSpawn()
    {
        SoundManager.instance.PlaySingle(spawnAudioClip);
        spawnParticleSystem.Play();
    }

    public virtual void OnDeath()
    {
        SoundManager.instance.PlaySingle(deathAudioClip);
        deathParticleSystem.Play();
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }


    // Update is called once per frame
    void Update () {
        OnUpdate();
	}

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & dieOnContactWith) != 0)
        {
            //It matched layer
            OnDeath();
        }
        
    }
}
