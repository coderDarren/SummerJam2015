using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SlowDown.AlterSpeed += AlterSpeed;
    }

    void OnDisable()
    {
        SlowDown.AlterSpeed -= AlterSpeed;
    }

    void AlterSpeed(float speedFactor)
    {
        if (speedFactor < 0.2f)
            audio.pitch = speedFactor * 2;
        else if (speedFactor < 0.2f)
            audio.pitch = speedFactor * 3;
        else
            audio.pitch = speedFactor;
    }
}
