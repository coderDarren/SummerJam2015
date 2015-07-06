using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BlockTransformAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Color transformationBlockedColor = Color.red;
    public Vector3 newRotation = Vector3.zero;

    BlockTransformer block;
    GameObject transformBlock;
    Vector3 targetEulers;
    Vector3 initialEulers;
    Vector3 currentEulers;
    bool applied = false;
    bool allowed = true;
    bool transforming = false;

    float xVel, yVel, zVel;

    void Start()
    {
        transformBlock = InteractiveCursor.InteractionObject;
        initialEulers = transformBlock.transform.localEulerAngles;
        targetEulers = initialEulers;
        block = transformBlock.GetComponent<BlockTransformer>();
        currentEulers.x = initialEulers.x;
        currentEulers.y = initialEulers.y;
        currentEulers.z = initialEulers.z;
    }

    

    void Update()
    {
        if (currentEulers != targetEulers)
        {
            transforming = true;
            currentEulers.x = Mathf.SmoothDampAngle(currentEulers.x, targetEulers.x, ref xVel, 5 * Time.deltaTime);
            currentEulers.y = Mathf.SmoothDampAngle(currentEulers.y, targetEulers.y, ref yVel, 5 * Time.deltaTime);
            currentEulers.z = Mathf.SmoothDampAngle(currentEulers.z, targetEulers.z, ref zVel, 5 * Time.deltaTime);

            if (Mathf.Abs(Vector3.Distance(currentEulers, targetEulers)) < 0.25f)
                currentEulers = targetEulers;

            transformBlock.transform.localEulerAngles = currentEulers;
        }
        else
        {
            transforming = false;
        }

        if (block.CollisionInChildren())
        {
            if (block.status != BlockTransformer.Status.Blocked) //if we are just now switching to blocked 
                block.SetChildrenColors(transformationBlockedColor);//we need to change the color of the blocks

            block.status = BlockTransformer.Status.Blocked;
            allowed = false;
        }
        else
        {
            if (block.status != BlockTransformer.Status.Unblocked) //if we are just now switching to unblocked 
                block.SetChildrenColors();//we need to change the color of the blocks

            block.status = BlockTransformer.Status.Unblocked;
            allowed = true;
        }
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        targetEulers = newRotation + initialEulers;
    }

    public void OnPointerExit(PointerEventData ped)
    {
        if (!applied)
            targetEulers = initialEulers;
        else
            applied = false; //must reset to false in case we come back to this button (we will want the rotation to be reset if we exit without applying)
    }

    public void OnPointerClick(PointerEventData ped)
    {
        
        if (allowed && !transforming)
        {
            initialEulers = transformBlock.transform.localEulerAngles;
            applied = true;
            block.ResetChildrenColors();
            Destroy(transform.parent.gameObject);
        }
        else
        {
            //play some noise perhaps (auditory que)
        }
    }

}
