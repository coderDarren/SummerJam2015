using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractableObject))]

public class BlockTransformer : MonoBehaviour {

    [HideInInspector]
    public enum Status { None, Unblocked, Blocked };
    public Status status;

    bool colliding = false;
    int collisionCount = 0; //there will be instances where you come in collision with two or more items. If you exit one collision but are still colliding with something else, you still want to be seen as colliding

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
            child.GetComponent<BlockTransformer>().status = Status.None;
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

    public void SetCollidersToTrigger(bool trigger)
    {
        BoxCollider[] bCols = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider col in bCols)
        {
            col.isTrigger = trigger;
        }
    }

    public bool CollisionInChildren()
    {
        BlockTransformer[] children = GetComponentsInChildren<BlockTransformer>();
        foreach (BlockTransformer child in children)
        {
            if (child.colliding)
            {
                return true;
            }
        }
        return false;
    }

    public void SetChildrenLayers(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);

        foreach (Transform child in obj.transform)
        {
            SetChildrenLayers(child.gameObject, layerName);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.tag != "Player")
        {
            collisionCount++;
            colliding = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.transform.tag != "Player")
        {
            if (collisionCount > 0)
                collisionCount--;

            if (collisionCount == 0)
            {
                colliding = false;
            }
        }
    }

}
