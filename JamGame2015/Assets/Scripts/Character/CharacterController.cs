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
        public LayerMask alternateGround;
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

    public bool underMagnetControl = false;

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput, jumpInput, walkInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;
    Ray groundCheckRay;

    float _forwardVel = 0;
    float _rotateVel = 0;
    float _jumpVel = 0;
    float _downAccel = 0;
    float reducedSpeed = 1;

    bool paused = false;

    public Quaternion TargetRotation { get { return targetRotation; } }
    public bool Paused { get { return paused; } }

    //public to be used by animator controller
    public bool Grounded()
    {
        groundCheckRay = new Ray(transform.position, Vector3.down);
        return Physics.SphereCast(groundCheckRay, moveSetting.groundCheckRadius, moveSetting.distToGrounded, moveSetting.ground);
        
    }
    public float ReducedSpeed { get { return reducedSpeed; } }


    void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("The character needs a rigidbody.");

        forwardInput = turnInput = jumpInput = walkInput = 0;

        _forwardVel = moveSetting.forwardVel;
        _rotateVel = moveSetting.rotateVel;
        _jumpVel = moveSetting.jumpVel;
        _downAccel = physSetting.downAccel;
    }

    //floats returned for the animator controller
    public float GetRunInput() { return forwardInput; }
    public float GetWalkInput() { return walkInput; }

    void GetInput()
    {
        forwardInput = Input.GetAxisRaw(inputSetting.FORWARD_AXIS); 
        turnInput = Input.GetAxisRaw(inputSetting.TURN_AXIS); 
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS); 
        walkInput = Input.GetAxisRaw(inputSetting.WALK_AXIS);
    }

    void ZeroAllInput()
    {
        forwardInput = 0;
        turnInput = 0;
        jumpInput = 0;
        walkInput = 0;
    }

    void Update()
    {
        if (!paused)
            GetInput();
        else
            ZeroAllInput();

        if (underMagnetControl)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, (transform.up * 2), 10 * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;

        if (underMagnetControl)
            return;

        if (!paused)
        {
            Run();
            Turn();
            Jump();

            rBody.velocity = transform.TransformDirection(velocity);
        }
        else
            rBody.velocity = Vector3.zero;

    }


    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            //move
            velocity.z = _forwardVel * forwardInput;
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
            targetRotation *= Quaternion.AngleAxis(_rotateVel * turnInput, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            //jump
            velocity.y = _jumpVel;
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
                velocity.y -= _downAccel;
        }
    }

    void OnEnable()
    {
        SlowDown.AlterSpeed += AlterSpeed;
        LevelManager.PausePlayer += PausePlayer;
        HelpWindow.PausePlayer += PausePlayer;
    }

    void OnDisable()
    {
        SlowDown.AlterSpeed -= AlterSpeed;
        LevelManager.PausePlayer -= PausePlayer;
        HelpWindow.PausePlayer -= PausePlayer;
    }


    void AlterSpeed(float speedFactor)
    {
       /* _forwardVel = moveSetting.forwardVel * speedFactor;
        if (speedFactor < 0.2f)
        {
            _rotateVel = moveSetting.rotateVel * (speedFactor * 1.5f);
            _jumpVel = moveSetting.jumpVel * (speedFactor * 3.2f);
        }
        else
        {
            _rotateVel = moveSetting.rotateVel * speedFactor;
            _jumpVel = moveSetting.jumpVel * speedFactor;
        }
        _downAccel = physSetting.downAccel * speedFactor;

        reducedSpeed = speedFactor;*/
    }

    void PausePlayer(bool paused)
    {
        this.paused = paused;
    }
}
