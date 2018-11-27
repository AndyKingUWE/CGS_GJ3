using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource AmbientSoundEffect;
    public AudioSource GameSoundEffect;
    public static SoundManager instance = null;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Use this for initialization
    void AwaKe  ( )
    {
        if (instance == null)
            instance = this;
        else if (instance != this) ;
        Destroy(gameObject);
	}
	public void  PlaySingle (AudioClip clip)
    {
        AmbientSoundEffect.clip = clip;
        AmbientSoundEffect.Play  ( );
    }

    public void RandomizeSfx (AudioClip clip)


	// Update is called once per frame
	void Update () {
		
	}
}
