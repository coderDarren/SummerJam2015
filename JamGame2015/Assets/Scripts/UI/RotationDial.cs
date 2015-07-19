using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotationDial : MonoBehaviour {

    RectTransform rt;
    float spriteRotation = 0;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        TopDownCamera.UpdateYOrbitValue += UpdateYOrbitValue;
    }

    void OnDisable()
    {
        TopDownCamera.UpdateYOrbitValue -= UpdateYOrbitValue;
    }
    
    void UpdateYOrbitValue(float degrees)
    {
        spriteRotation = degrees + 180; //cam rotation starts at -180, sprite rotation starts at 0
        rt.eulerAngles = new Vector3(rt.eulerAngles.x, rt.eulerAngles.y, -spriteRotation);
    }
}
