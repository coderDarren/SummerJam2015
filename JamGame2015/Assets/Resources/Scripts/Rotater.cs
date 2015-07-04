using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {

    public Vector3 rotateAxis = new Vector3(0, 1, 0);
    public float rotateSmooth = 5f;

    void FixedUpdate()
    {
        transform.rotation *= Quaternion.AngleAxis(rotateSmooth, rotateAxis);
    }
}
