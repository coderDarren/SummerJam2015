using UnityEngine;
using System.Collections;

public class EndLevelCheckPoint : MonoBehaviour {

    public delegate void CompletionHandler();
    public static event CompletionHandler EndLevel;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            EndLevel();
        }
    }
}
