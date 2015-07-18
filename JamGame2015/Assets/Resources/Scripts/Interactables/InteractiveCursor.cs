using UnityEngine;
using System.Collections;

public class InteractiveCursor : MonoBehaviour
{

    #region Interaction Events Here!

    public delegate void BlockTransformerClick(GameObject block);
    public static event BlockTransformerClick OpenTransformOptions;
    public static event BlockTransformerClick CloseTransformOptions;
    public delegate void PickUpClick (GameObject item);
    public static event PickUpClick PickupItem;
    public static event PickUpClick DropItem;

    #endregion

    public static GameObject InteractionObject;

    public Texture2D baseCursorTex;
    public Texture2D interactCursorTex;
    public Texture2D hostileCursorTex;
    public string INTERACT_INPUT = "Fire1";
    public float cursorSize = 32;

    bool onInteractable = false;
    Texture2D cursorTex;
    GameObject objectUnderCursor;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        cursorTex = baseCursorTex;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        InteractableObject.InteractWith += InteractWith;
    }

    void OnDisable()
    {
        InteractableObject.InteractWith -= InteractWith;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (cursorSize / 2), Event.current.mousePosition.y - (cursorSize / 2), cursorSize, cursorSize), cursorTex);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (onInteractable && objectUnderCursor != null)
        {
            if (Clicked(objectUnderCursor))
            {
                HandleInteractions();
            }
        }
        else
        {
            
        }
    }

    void HandleInteractions()
    {
        if (!InteractionObject)
        {
            InteractionObject = objectUnderCursor; //only for first interaction
        }

        //below here, check for all possible interaction objects and handle their interactions accordingly

        if (objectUnderCursor.GetComponent<BlockTransformer>()) //a block transform interaction
        {
            //reset last selection colors
            if (InteractionObject.GetComponent<BlockTransformer>())
                InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
            //set new interaction object
            InteractionObject = objectUnderCursor;
            //set new selection colors
            InteractionObject.GetComponent<BlockTransformer>().SetChildrenColors();
            //reset selection if you are clicking what your cursor is already on
            OpenTransformOptions(InteractionObject);
        }
        else
        {
            CloseTransformOptions(InteractionObject);
        }
        if (objectUnderCursor.GetComponent<Magnet>())
        {
            InteractionObject = objectUnderCursor;
            PickupItem(InteractionObject);
        }
        else
        {
            try
            {
                DropItem(InteractionObject);
            }
            catch (System.NullReferenceException)
            { }
        }
    }

    bool Clicked(GameObject checkObject)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == objectUnderCursor)
            {
                if (Input.GetAxis(INTERACT_INPUT) > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void InteractWith(bool canInteract, GameObject interactObject)
    {
        onInteractable = canInteract;
        if (canInteract)
        {
            objectUnderCursor = interactObject; //setting this - will check to see if it is clicked - if so, we set the interaction object to objectUnderCursor
            cursorTex = interactCursorTex;
        }
        else
            cursorTex = baseCursorTex;
    }
}
