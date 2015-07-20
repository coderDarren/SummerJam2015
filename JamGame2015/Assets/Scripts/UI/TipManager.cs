using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TipManager : MonoBehaviour {

    public string[] tips;
    public GameObject TipInterface;
    public float tipFrequency = 80;

    GameObject tipInterface;
    Image[] images;
    Text tip;
    float frequencyTimer = 0;
    float colorAlpha = 1;
    int tipIndex = 0;

    void Start()
    {
        SpawnNewTip();
    }

    void Update()
    {
        RunTimers();

        if (frequencyTimer >= tipFrequency)
        {
            SpawnNewTip();
            frequencyTimer = 0;
        }

        if (tipInterface)
        {
            if (tip.color.a <= 0.1f)
            {
                Destroy(tipInterface);
                frequencyTimer = 0;
                colorAlpha = 1;
            }
        }
    }

    void RunTimers()
    {
        frequencyTimer += Time.deltaTime;
    }

    void SpawnNewTip()
    {
        tipInterface = Instantiate(TipInterface as GameObject);
        tipInterface.transform.parent = transform;
        images = tipInterface.GetComponentsInChildren<Image>();
        tip = tipInterface.GetComponentInChildren<Text>();
        tip.text = tips[tipIndex];

        tipIndex++;
        if (tipIndex > tips.Length - 1)
            tipIndex = 0;
    }

    
}
