using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    AudioSource audio;
    float basePitch = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        basePitch = audio.pitch;
    }

    void OnEnable()
    {
        SlowDown.AlterSpeed += AlterSpeed;
        LevelCompleteButtons.RestartLevel += RestartLevel;
    }

    void OnDisable()
    {
        SlowDown.AlterSpeed -= AlterSpeed;
        LevelCompleteButtons.RestartLevel -= RestartLevel;
    }

    void AlterSpeed(float speedFactor)
    {
        if (speedFactor < 0.2f)
            audio.pitch = basePitch * speedFactor * 2;
        else if (speedFactor < 0.2f)
            audio.pitch = basePitch * speedFactor * 3;
        else
            audio.pitch = basePitch * speedFactor;
    }
    
    void RestartLevel()
    {
        audio.pitch = basePitch;
    }
}
