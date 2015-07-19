using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class BloomControl : MonoBehaviour {

    public float baseIntensity = 2;
    public float intensitySmooth = 10;
    Bloom bloom;

    void Start()
    {
        bloom = GetComponent<Bloom>();
    }

    void Update()
    {
        if (bloom.bloomIntensity != baseIntensity)
        {
            RegulateIntensity();
        }
    }

    void RegulateIntensity()
    {
        bloom.bloomIntensity = Mathf.Lerp(bloom.bloomIntensity, baseIntensity, intensitySmooth * Time.deltaTime);
        if (Mathf.Abs(bloom.bloomIntensity - baseIntensity) < 1)
            bloom.bloomIntensity = baseIntensity;
    }

    public void BoostIntensity(float newIntensity)
    {
        bloom.bloomIntensity = newIntensity;
    }
}
