using UnityEngine;
using System.Collections;

public class HelpWindowTrigger : MonoBehaviour {

    public delegate void HelpWindowHandler(string windowName);
    public static event HelpWindowHandler OpenWindow;

    public string windowToOpen = "Enter Window Name Here";

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            OpenWindow(windowToOpen); //HelpWindowManager.cs listens for this
        }
    }
}
