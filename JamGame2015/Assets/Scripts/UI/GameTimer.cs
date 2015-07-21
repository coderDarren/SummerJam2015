using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    public delegate void TimeSender(int hours, int minutes, int seconds);
    public static event TimeSender GetFinalTime;

    Text text;

    float totalMilliseconds = 0;
    float startMilliseconds = 0;
    int seconds = 0;
    int minutes = 0;
    int hours = 0;

    bool running = false;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = "00:00:00";
    }

    void Update()
    {
        if (running)
        {
            totalMilliseconds = Time.timeSinceLevelLoad * 1000 + startMilliseconds;
            seconds = (int)(totalMilliseconds / 1000) - (minutes * 60) - (hours * 60 * 60);
            minutes = (int)(totalMilliseconds / 60000) - (hours * 60);
            hours = (int)totalMilliseconds / (60000 * 60);
        }

        text.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    void OnEnable()
    {
        LevelManager.StartTimeAt += StartTimeAt;
        LevelManager.StopTime += StopTime;
    }

    void OnDisable()
    {
        LevelManager.StartTimeAt -= StartTimeAt;
        LevelManager.StopTime -= StopTime;
    }

    void StartTimeAt(int minutes, int seconds)
    {
        startMilliseconds = (1000 * seconds) + (60000 * minutes);
        running = true;
    }

    void StopTime()
    {
        running = false;
        GetFinalTime(hours, minutes, seconds);
    }
    
}
