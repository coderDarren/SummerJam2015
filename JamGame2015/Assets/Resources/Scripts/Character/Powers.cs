using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Powers : MonoBehaviour {

    public GameObject PushObject;
    public Transform PushLocation;
    public float pushCooldown = 2;
    public string PUSH_AXIS = "Push";

    float pushInput = 0;
    float pushTimer = 0;

    //animation floats
    public float GetPushInput() { return pushInput; }

    //this reference can help us determine if our player is moving
    //the player cannot use powers if moving
    CharacterController character; 

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    bool CanPush() 
    {
        if (pushTimer > pushCooldown && character.GetRunInput() < 0.1f)
            return true;
        return false;
    }

    void GetInput()
    {
        if (CanPush())
           pushInput = Input.GetAxis(PUSH_AXIS);
    }

    void RunTimers()
    {
        if (pushTimer <= pushCooldown)
            pushTimer += Time.deltaTime;
    }

    void Update()
    {
        GetInput();
        RunTimers();

        Debug.Log("push: " +pushInput);

        if (CanPush() && pushInput > 0)
        {
            ActivatePush();
            pushInput = 0;
            pushTimer = 0;
        }
    }

    void ActivatePush()
    {
        Instantiate(PushObject, PushLocation.position, Quaternion.identity);
    }

}
