using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockTransformAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public delegate void RotateBlock(Vector3 target);
    public static event RotateBlock SetTargetRotation;
    public delegate void ResetBlock();
    public static event ResetBlock ResetBlocks;
    public delegate void ClickHandler();
    public static event ClickHandler HandleButtonClick;

    public Vector3 newRotation;
    public bool cancelButton = false;

    public void OnPointerEnter(PointerEventData ped)
    {
        if (GetComponent<Button>().interactable)
        {
            if (!cancelButton)
            {
                SetTargetRotation(newRotation);
            }
        }
    }

    public void OnPointerExit(PointerEventData ped)
    {
        if (GetComponent<Button>().interactable)
        {
            ResetBlocks();
        }
    }

    public void OnPointerClick(PointerEventData ped)
    {
        if (GetComponent<Button>().interactable)
        {
            if (cancelButton)
                ResetBlocks();
            HandleButtonClick();
        }
    }

    
}
