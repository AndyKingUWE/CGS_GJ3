using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Lantern : MonoBehaviour {

    [SerializeField] VRTK_InteractableObject linkedObject;
    [SerializeField] Light lantern_light;

    [SerializeField] float flicker_intensity;
    [SerializeField] float light_intensity;

    [SerializeField] Material lit_mat;
    [SerializeField] Material unlit_mat;

    private bool lit = false;
    private bool stop_lit = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (linkedObject.IsGrabbed() && !lit)
        {
            LanternOn();
        }

        else if (!linkedObject.IsGrabbed() && lit)
        {
            LanternOff();
        }

        if (!stop_lit && !lit)
        {
            StartCoroutine(Flicker());
        }

        if (!lit && lantern_light.intensity > 0)
        {
            lantern_light.intensity -= 0.015f;
        }
    }

    void LanternOn()
    {
        lantern_light.intensity = light_intensity;
        GetComponent<MeshRenderer>().materials[1] = lit_mat;
        stop_lit = false;
    }

    void LanternOff()
    {
        stop_lit = true;
        GetComponent<MeshRenderer>().materials[1] = unlit_mat;
    }

    private IEnumerator Flicker()
    {
        float flicker = 0.2f;
        float dampen = 0.1f;

        lit = true;
        while (!stop_lit)
        {
            lantern_light.intensity = Mathf.Lerp(lantern_light.intensity, Random.Range(light_intensity - flicker, light_intensity + flicker), flicker_intensity * Time.deltaTime);
            yield return new WaitForSeconds(dampen);
        }
        lit = false;
    }
}
