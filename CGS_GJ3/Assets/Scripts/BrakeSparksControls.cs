using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeSparksControls : MonoBehaviour
{
    private ParticleSystem[] sparksParticleSystems;

	// Use this for initialization
	void Start () {
	    sparksParticleSystems = GetComponentsInChildren<ParticleSystem>();
	    foreach (ParticleSystem particle in sparksParticleSystems)
	    {
            particle.Stop();
	    }
    }

    void StartBrakeSparks()
    {
        foreach (ParticleSystem particle in sparksParticleSystems)
        {
            particle.Play();
        }
    }

    void StopBrakeSparks()
    {
        foreach (ParticleSystem particle in sparksParticleSystems)
        {
            particle.Stop();
        }
    }
}
