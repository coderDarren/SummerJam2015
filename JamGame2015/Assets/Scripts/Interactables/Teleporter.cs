using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public Transform teleportLocation;

    BloomControl bloomControl;

    void Start()
    {
        bloomControl = Camera.main.GetComponent<BloomControl>();
    }

    void Teleport(GameObject teleportee)
    {
        bloomControl.BoostIntensity(100);
        teleportee.transform.position = teleportLocation.position + Vector3.up * 1.5f;
    }

    void OnEnable()
    {
        InteractiveCursor.Teleport += Teleport;
    }

    void OnDisable()
    {
        InteractiveCursor.Teleport -= Teleport;
    }
}
