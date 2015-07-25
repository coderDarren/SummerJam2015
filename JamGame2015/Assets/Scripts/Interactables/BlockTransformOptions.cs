using UnityEngine;
using System.Collections;

public class BlockTransformOptions : MonoBehaviour
{
    public GameObject OptionsMenu;
    public Color transformationBlockedColor = Color.red;
    public Color transformationUnblockedColor = Color.blue;

    BlockTransformer block;
    GameObject transformBlock;
    Quaternion targetRotation;
    Quaternion initialRotation;
    Quaternion currentRotation;
    bool allowed = true;
    bool transforming = false;
    bool initialized = false;

    GameObject menu;
    float xVel, yVel, zVel;


    #region Transforming Blocks

    void Initialize()
    {
        transformBlock = InteractiveCursor.InteractionObject;
        block = transformBlock.GetComponent<BlockTransformer>();

        initialRotation = transformBlock.transform.rotation;
        targetRotation = initialRotation;
        currentRotation = initialRotation;

        initialized = true;
    }

    void Update()
    {
        if (initialized)
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

            if (block != null)
                CheckCollisions();
        }
    }

    void SetTargetRotation(Vector3 newRotation)
    {
        targetRotation = Quaternion.Euler(newRotation) * initialRotation;
    }

    void HandleButtonClick()
    {
        if (allowed && !transforming)
        {
            CancelTransformation();
            initialRotation = transformBlock.transform.rotation;
            targetRotation = initialRotation;
            currentRotation = initialRotation;
            //play some noise perhaps (auditory que)
        }
        else
        {
            //play some noise perhaps (auditory que)
        }
    }

    void ResetBlocks()
    {
        transformBlock.transform.rotation = initialRotation;
        targetRotation = initialRotation;
    }

    void CheckCollisions()
    {
        if (block.CollisionInChildren())
        {
            if (block.status != BlockTransformer.Status.Blocked) //if we are just now switching to blocked 
            {
                block.SetChildrenColors(transformationBlockedColor);//we need to change the color of the blocks
            }

            block.status = BlockTransformer.Status.Blocked;
            allowed = false;
            block.SetCollidersToTrigger(true);
            block.SetChildrenLayers(block.gameObject, "TransformBlockColliding");
        }
        else
        {
            if (block.status != BlockTransformer.Status.Unblocked) //if we are just now switching to unblocked 
            {
                block.SetChildrenColors(transformationUnblockedColor);//we need to change the color of the blocks
            }

            block.status = BlockTransformer.Status.Unblocked;
            allowed = true;
            block.SetCollidersToTrigger(false);
            block.SetChildrenLayers(block.gameObject, "TransformBlock");
        }
    }

    #endregion

    #region UI

    void OpenTransformOptions(GameObject blockObject)
    {
        if (menu)
            Destroy(menu);
        menu = Instantiate(OptionsMenu, new Vector3(Screen.width / 2, Screen.height / 2, 0) + Vector3.right * (Screen.width / 6), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
        Initialize();
    }

    void CancelTransformation()
    {
        Destroy(menu);
        InteractiveCursor.InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
        InteractiveCursor.InteractionObject = null;
        initialized = false;
    }

    #endregion

    void OnEnable()
    {
        InteractiveCursor.OpenTransformOptions += OpenTransformOptions;
        InteractiveCursor.CloseTransformOptions += CancelTransformation;
        BlockTransformAction.HandleButtonClick += HandleButtonClick;
        BlockTransformAction.SetTargetRotation += SetTargetRotation;
        BlockTransformAction.ResetBlocks += ResetBlocks;
    }

    void OnDisable()
    {
        InteractiveCursor.OpenTransformOptions -= OpenTransformOptions;
        InteractiveCursor.CloseTransformOptions -= CancelTransformation;
        BlockTransformAction.HandleButtonClick -= HandleButtonClick;
        BlockTransformAction.SetTargetRotation -= SetTargetRotation;
        BlockTransformAction.ResetBlocks -= ResetBlocks;
    }
}
