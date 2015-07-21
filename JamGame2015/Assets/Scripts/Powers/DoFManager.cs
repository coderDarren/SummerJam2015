using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(DepthOfField))]

public class DoFManager : MonoBehaviour {

    public float maxAperture = 4.5f;
    public float apertureSmooth = 1;

    DepthOfField dof;
    bool active = false;

    void Start()
    {
        dof = GetComponent<DepthOfField>();
    }

    void Update()
    {
        if (!active)
        {
            if (dof.aperture > 0)
            {
                dof.aperture -= apertureSmooth * Time.deltaTime;
            }
        }
        else
        {
            if (dof.aperture < maxAperture)
            {
                dof.aperture += apertureSmooth * Time.deltaTime;
            }
        }
    }

    void OnEnable()
    {
        SlowDown.SlowDownActive += SlowDownActive;
    }

    void OnDisable()
    {
        SlowDown.SlowDownActive -= SlowDownActive;
    }

    void SlowDownActive(bool active)
    {
        this.active = active;
    }
}
