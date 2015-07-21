using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCompleteStats : MonoBehaviour {

    public Text finalTime;

    void OnEnable()
    {
        GameTimer.GetFinalTime += GetFinalTime;
    }

    void OnDisable()
    {
        GameTimer.GetFinalTime -= GetFinalTime;
    }

    void GetFinalTime(int hours, int minutes, int seconds)
    {
        finalTime.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
