using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour {

    public enum Behavior { TextPulsate, ButtonPulsate };
    public Behavior behavior;
    public GameObject textObj;
    public Color pulseColor;
    public float pulseSpeed = 1;

    Text txt;
    Color initialColor;
    Color currColor;
    bool pulsed = false;

    void Start()
    {
        txt = textObj.GetComponent<Text>();
        currColor = txt.color;
        initialColor = currColor;
    }

    void Update()
    {
        switch(behavior)
        {
            case Behavior.TextPulsate: 
            case Behavior.ButtonPulsate: 
                UpdatePulsate(); 
                break;
        }
    }


    void UpdatePulsate()
    {
        /*if (pulseColor.a - currColor.a > 0.05f && !pulsed)
        {
            currColor = Vector4.Lerp(currColor, pulseColor, pulseSpeed * Time.deltaTime);
        }
        else
            pulsed = true;

        if (pulsed)
        {
            currColor = Vector4.Lerp(currColor, initialColor, pulseSpeed * Time.deltaTime);

            if (pulseColor.a - currColor.a > 0.9f)
                pulsed = false;
        }*/
        currColor.a = Mathf.PingPong(Time.time, 0.5f);

        txt.color = currColor;
    }
}
