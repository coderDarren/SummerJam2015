using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public Vector3 teleportLocation;

    BloomControl bloomControl;

    void Start()
    {
        bloomControl = Camera.main.GetComponent<BloomControl>();
    }

    public void Teleport(GameObject teleportee)
    {
        bloomControl.BoostIntensity(50);
        teleportee.transform.position = teleportLocation;
    }
}
