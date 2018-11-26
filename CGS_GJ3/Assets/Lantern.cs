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
    private Material[] mats;

    // Use this for initialization
    void Start () {
        mats = GetComponent<MeshRenderer>().materials;

    }
	
	// Update is called once per frame
	void Update () {

        if ((linkedObject.IsGrabbed() || linkedObject.IsTouched()) && !lit)
        {
            LanternOn();
        }

        else if ((!linkedObject.IsGrabbed() && !linkedObject.IsTouched()) && lit)
        {
            LanternOff();
        }

        if (!stop_lit && !lit)
        {
            StartCoroutine(Flicker());
        }

        if (!lit && lantern_light.intensity > 0)
        {
            lantern_light.intensity -= 0.025f;
        }
    }

    void LanternOn()
    {
        lantern_light.intensity = light_intensity;

        mats[1] = lit_mat;

        GetComponent<MeshRenderer>().materials = mats;

        stop_lit = false;
    }

    void LanternOff()
    {
        stop_lit = true;

        mats[1] = unlit_mat;

        GetComponent<MeshRenderer>().materials = mats;
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
