﻿using UnityEngine;
using System.Collections;

public class TransformButtonRotationHandler : MonoBehaviour {

    RectTransform rt;
    float spriteRotationZ = 0;
    float spriteRotationX = 0;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        TopDownCamera.UpdateYOrbitValue += UpdateYOrbitValue;
        TopDownCamera.UpdateXOrbitValue += UpdateXOrbitValue;
    }

    void OnDisable()
    {
        TopDownCamera.UpdateYOrbitValue -= UpdateYOrbitValue;
        TopDownCamera.UpdateXOrbitValue -= UpdateXOrbitValue;
    }

    void UpdateYOrbitValue(float degrees)
    {
        spriteRotationZ = -degrees + 180; //cam rotation starts at -180, sprite rotation starts at 0
        rt.eulerAngles = new Vector3(spriteRotationX, rt.eulerAngles.y, -spriteRotationZ);
    }

    void UpdateXOrbitValue(float degrees)
    {
        spriteRotationX = degrees; //cam rotation starts at -180, sprite rotation starts at 0
        rt.eulerAngles = new Vector3(spriteRotationX, rt.eulerAngles.y, rt.eulerAngles.z);
    }
}
