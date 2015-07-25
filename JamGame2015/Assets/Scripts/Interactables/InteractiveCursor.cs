using UnityEngine;
using System.Collections;

public class InteractiveCursor : MonoBehaviour
{

    #region Interaction Events Here!

    public delegate void ObjectClick(GameObject obj);
    public static event ObjectClick OpenTransformOptions;
    public static event ObjectClick OpenMagnetOptions;
    public static event ObjectClick PickupItem;
    public static event ObjectClick DropItem;
    public delegate void CancelTransformerOptions();
    public static event CancelTransformerOptions CloseTransformOptions;
    public delegate void CancelMagnetOptions();
    public static event CancelMagnetOptions CloseMagnetOptions;
    public delegate void TeleportHandler(GameObject teleportee, Vector3 teleportPos);
    public static event TeleportHandler Teleport;

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

    float teleportCD = 2; //teleport cooldown
    float teleportTimer = 0;


    void Start()
    {
        cursorTex = baseCursorTex;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        InteractableObject.InteractWith += InteractWith;
        LevelCompleteButtons.RestartLevel += RestartLevel;
    }

    void OnDisable()
    {
        InteractableObject.InteractWith -= InteractWith;
        LevelCompleteButtons.RestartLevel -= RestartLevel;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (cursorSize / 2), Event.current.mousePosition.y - (cursorSize / 2), cursorSize, cursorSize), cursorTex);
    }

    void Update()
    {
        if (teleportTimer < teleportCD)
            teleportTimer += Time.deltaTime;
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
            if (InteractionObject.GetComponent<Magnet>())
                CloseMagnetOptions();
            //reset last selection colors
            if (InteractionObject.GetComponent<BlockTransformer>())
                InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
            //set new interaction object
            InteractionObject = objectUnderCursor;
            //set new selection colors
            InteractionObject.GetComponent<BlockTransformer>().SetChildrenColors();
            //reset selection if you are clicking what your cursor is already on
            OpenTransformOptions(InteractionObject);
            Debug.Log("..");
            return;
        }
        if (objectUnderCursor.GetComponent<Magnet>()) //a magnet interaction
        {
            if (InteractionObject.GetComponent<BlockTransformer>())
                CloseTransformOptions();
            InteractionObject = objectUnderCursor;
            OpenMagnetOptions(InteractionObject);
            
            return;
        }
        if (objectUnderCursor.GetComponent<Teleporter>())
        {
            if (teleportTimer >= teleportCD) //helps prevent confusing teleport interactions
            {
                Teleport(gameObject, objectUnderCursor.GetComponent<Teleporter>().teleportLocation.position);
                teleportTimer = 0;
            }
        }

        try
        {
            CloseTransformOptions();
        }
        catch (System.NullReferenceException)
        { }
        try
        {
            CloseMagnetOptions();
        }
        catch (System.NullReferenceException)
        { }
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

    void RestartLevel()
    {
        InteractionObject = null;
        try
        {
            CloseMagnetOptions();
            CloseTransformOptions();
        }
        catch (System.NullReferenceException) { }
    }
}
