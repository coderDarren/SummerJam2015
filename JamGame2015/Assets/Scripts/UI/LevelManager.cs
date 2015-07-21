using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public delegate void TimerHandler(int minutes, int seconds);
    public delegate void TimeHandler();
    public static event TimerHandler StartTimeAt;
    public static event TimeHandler StopTime;

    public static LevelManager Instance;

    public GameObject LevelCompleteMenu;
    GameObject completeMenu;
    string finalTime;

    string level_name;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        StartTimeAt(0, 0);
        level_name = Application.loadedLevelName;
    }

    void OnEnable()
    {
        EndLevelCheckPoint.EndLevel += EndLevel;
        LevelCompleteButtons.QuitApplication += QuitApplication;
        LevelCompleteButtons.RestartLevel += RestartLevel;
    }

    void OnDisable()
    {
        EndLevelCheckPoint.EndLevel -= EndLevel;
        LevelCompleteButtons.QuitApplication -= QuitApplication;
        LevelCompleteButtons.RestartLevel -= RestartLevel;
    }

    void EndLevel()
    {
        //spawn level complete menu
        //fill display values for the menu
        completeMenu = Instantiate(LevelCompleteMenu as GameObject);
        completeMenu.transform.parent = transform;
        completeMenu.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        StopTime();
    }

    void RestartLevel()
    {
        if (completeMenu)
            Destroy(completeMenu);
        StartTimeAt(0, 0);
        Application.LoadLevel(level_name);
    }

    void QuitApplication()
    {
        Application.Quit();
    }
}
