using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractableObject))]

public class BlockTransformer : MonoBehaviour {


    public void ResetChildrenColors()
    {
        InteractableObject[] children = GetComponentsInChildren<InteractableObject>();
        foreach (InteractableObject child in children)
        {
            child.ResetMaterialColors();
            child.selected = false;
        }
    }

	public void SetChildrenColors()
    {
        InteractableObject[] children = GetComponentsInChildren<InteractableObject>();
        foreach(InteractableObject child in children)
        {
            child.SetMaterialColors(child.interactColor);
            child.selected = true;
        }
    }
}
