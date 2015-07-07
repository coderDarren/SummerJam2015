using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BlockTransformAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Color transformationBlockedColor = Color.red;
    public Vector3 newRotation;

    BlockTransformer block;
    GameObject transformBlock;
    Quaternion targetRotation;
    Quaternion initialRotation;
    Quaternion currentRotation;
    bool applied = false;
    bool allowed = true;
    bool transforming = false;

    float xVel, yVel, zVel;

    void Start()
    {
        transformBlock = InteractiveCursor.InteractionObject;
        block = transformBlock.GetComponent<BlockTransformer>();

        initialRotation = transformBlock.transform.rotation;
        targetRotation = initialRotation;
        currentRotation = initialRotation;
    }

    

    void Update()
    {
        if (currentRotation != targetRotation)
        {
            transforming = true;

            currentRotation = Quaternion.Lerp(currentRotation, targetRotation, 10 * Time.deltaTime);
            transformBlock.transform.rotation = currentRotation;
        }
        else
        {
            transforming = false;
        }

        CheckCollisions();
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        targetRotation = Quaternion.Euler(newRotation) * initialRotation;
    }

    public void OnPointerExit(PointerEventData ped)
    {
        if (!applied)
            targetRotation = initialRotation;
        else
            applied = false; //must reset to false in case we come back to this button (we will want the rotation to be reset if we exit without applying)
    }

    public void OnPointerClick(PointerEventData ped)
    {
        
        if (allowed && !transforming)
        {
            applied = true;
            block.ResetChildrenColors();
            InteractiveCursor.InteractionObject = null;
            Destroy(transform.parent.parent.gameObject);
        }
        else
        {
            //play some noise perhaps (auditory que)
        }
    }


    void CheckCollisions()
    {
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
}
