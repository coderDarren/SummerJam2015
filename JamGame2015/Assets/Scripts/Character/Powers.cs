using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Powers : MonoBehaviour {

    public GameObject PushObject;
    public Transform PushLocation;
    public float pushCooldown = 4;
    public string PUSH_AXIS = "Push";

    float pushInput = 0;
    float pushTimer = 0;
    float pushDuration = 0.7f;
    float animateDurationTimer = 0;
    bool holding = false;
    GameObject pickUpObject;

    //animation variables
    public bool Pushing() 
    {
        if (animateDurationTimer > 0)
            return true;
        return false;
    }
    public bool Holding() { return holding; }

    //this reference can help us determine if our player is moving
    //the player cannot use powers if moving
    CharacterController character; 

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    bool CanPush() 
    {
        if (pushTimer > pushCooldown && character.GetRunInput() < 0.1f && !Holding())
            return true;
        return false;
    }

    void GetInput()
    {
        pushInput = Input.GetAxisRaw(PUSH_AXIS);
    }

    void RunTimers()
    {
        if (pushTimer <= pushCooldown)
            pushTimer += Time.deltaTime;
        if (animateDurationTimer >= 0)
            animateDurationTimer -= Time.deltaTime;
    }

    void Update()
    {
        GetInput();
        RunTimers();
       
        if (CanPush() && pushInput > 0)
        {
            ActivatePush();
            pushTimer = 0;
            animateDurationTimer = pushDuration;
        }

        if (character.GetRunInput() > 0.1f)
        {
            animateDurationTimer = 0;
        }

        if (Holding())
        {
            PickObjectUp();
        }
        Debug.Log(Holding());
    }

    void ActivatePush()
    {
        Instantiate(PushObject, PushLocation.position, Quaternion.identity);
    }

    void PickObjectUp()
    {
        pickUpObject.transform.localPosition = Vector3.Lerp(pickUpObject.transform.localPosition, (Vector3.up * 2 + Vector3.forward * 8), 5 * Time.deltaTime);
    }

    void OnEnable()
    {
        InteractiveCursor.PickupItem += HoldObject;
        InteractiveCursor.DropItem += DropObject;
    }

    void OnDisable()
    {
        InteractiveCursor.PickupItem -= HoldObject;
        InteractiveCursor.DropItem -= DropObject;
    }

    void HoldObject(GameObject obj)
    {
        pickUpObject = obj;
        obj.transform.parent = transform;
        holding = true;
        pickUpObject.GetComponent<Rigidbody>().useGravity = false;
    }

    void DropObject(GameObject obj)
    {
        pickUpObject.GetComponent<Rigidbody>().useGravity = true;
        pickUpObject.transform.parent = null;
        pickUpObject = null;
        holding = false;
    }

}
