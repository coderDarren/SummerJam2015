using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpWindowManager : MonoBehaviour {

    public GameObject[] windows;

    GameObject activeWindow;
    bool showHelp = true; //for all help triggers
    bool[] showWindowAtIndexAgain; //for individual triggers

    void Start()
    {
        showWindowAtIndexAgain = new bool[windows.Length];
        for (int i = 0; i < showWindowAtIndexAgain.Length; i++)
            showWindowAtIndexAgain[i] = true;
    }

    void OpenWindow(string windowName)
    {
        if (showHelp)
        {
            for (int i = 0; i < windows.Length; i++)
            {
                HelpWindow help = windows[i].GetComponent<HelpWindow>();
                if (help.windowName == windowName)
                {
                    if (showWindowAtIndexAgain[i]) //if this window's "showAgain" toggle is true
                    {
                        //otherwise, if it has been opened and if showAgain is true, open again
                        if (activeWindow)
                            Destroy(activeWindow);
                        GameObject w = Instantiate(windows[i], new Vector3(Screen.width / 2, Screen.height / 2, 0) + Vector3.left * (Screen.width / 4), Quaternion.identity) as GameObject;
                        w.transform.parent = transform;
                        w.GetComponent<RectTransform>().localScale = Vector3.one;
                        activeWindow = w;
                    }
                }
            }
        }
    }

    void SetViewable(string windowName, bool viewable)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].GetComponent<HelpWindow>().windowName == windowName)
            {
                showWindowAtIndexAgain[i] = false;
            }
        }
    }

    void ToggleShowHelp()
    {
        showHelp = !showHelp;
    }

    void OnEnable()
    {
        HelpWindowTrigger.OpenWindow += OpenWindow;
        HelpWindow.SetViewable += SetViewable;
    }

    void OnDisable()
    {
        HelpWindowTrigger.OpenWindow -= OpenWindow;
        HelpWindow.SetViewable -= SetViewable;
    }
}
