using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LevelCompleteButtons : MonoBehaviour, IPointerClickHandler {

    public delegate void ButtonHandler();
    public static event ButtonHandler RestartLevel;
    public static event ButtonHandler QuitApplication;

    public enum Button { Retry, Quit };
    public Button button;


    public void OnPointerClick(PointerEventData ped)
    {
        switch (button)
        {
            case Button.Retry: RestartLevel(); break;
            case Button.Quit: QuitApplication(); break;
        }
    }

}
