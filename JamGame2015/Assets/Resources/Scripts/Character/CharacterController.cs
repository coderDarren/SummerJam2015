using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 3;
        public float mouseRotateVel = 0.1f;
        public float jumpVel = 5;
        public float distToGrounded = 0.1f;
        public float groundCheckRadius = 0.25f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.3f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public string WALK_AXIS = "Walk";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput, jumpInput, walkInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;
    Ray groundCheckRay;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        groundCheckRay = new Ray(transform.position, Vector3.down);
        return Physics.SphereCast(groundCheckRay, moveSetting.groundCheckRadius, moveSetting.distToGrounded, moveSetting.ground);
    }

    void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("The character needs a rigidbody.");

        forwardInput = turnInput = jumpInput = walkInput = 0;
    }

    //floats returned for the animator controller
    public float GetRunInput() { return forwardInput; }
    public float GetJumpInput() { return jumpInput; }
    public float GetWalkInput() { return walkInput; }

    void GetInput()
    {
        forwardInput = Input.GetAxisRaw(inputSetting.FORWARD_AXIS); 
        turnInput = Input.GetAxisRaw(inputSetting.TURN_AXIS); 
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); 
        walkInput = Input.GetAxisRaw(inputSetting.WALK_AXIS);
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;

        Run();
        Turn();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);
    }


    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            //move
            velocity.z = moveSetting.forwardVel * forwardInput;
            if (walkInput > 0)
                velocity.z /= 2.5f;
        }
        else
            //zero velocity
            velocity.z = 0;

    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            //jump
            velocity.y = moveSetting.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            //zero out our velocity.y
            velocity.y = 0;
        }
        else
        {
            //decrease velocity.y
            if (velocity.y > -10)
                velocity.y -= physSetting.downAccel;
        }
    }
}
