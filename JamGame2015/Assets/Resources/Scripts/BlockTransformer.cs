using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractableObject))]

public class BlockTransformer : MonoBehaviour {

    [HideInInspector]
    public enum Status { Unblocked, Blocked };
    public Status status;

    bool colliding = false;

    public bool Colliding
    {
        get { return colliding; }
    }

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

    public void SetChildrenColors(Color toColor)
    {
        InteractableObject[] children = GetComponentsInChildren<InteractableObject>();
        foreach (InteractableObject child in children)
        {
            child.SetMaterialColors(toColor);
            child.selected = true;
        }
    }

    public bool CollisionInChildren()
    {
        BlockTransformer[] children = GetComponentsInChildren<BlockTransformer>();
        foreach (BlockTransformer child in children)
        {
            if (child.colliding)
                return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.tag == "TransformBlock")
        {
            Debug.Log("Collision detected.");
            colliding = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.transform.tag == "TransformBlock")
        {
            Debug.Log("Collision exited.");
            colliding = false;
        }
    }

}
