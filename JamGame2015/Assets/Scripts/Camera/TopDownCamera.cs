using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour {

    public delegate void OrbitControl(float degrees);
    public static event OrbitControl UpdateYOrbitValue;

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        //distance from our target
        //bools for zooming and smoothfollowing
        //min and max zoom settings
        public Vector3 targetPosOffset = new Vector3(0, 0, 0);
        public float distanceFromTarget = -50;
        public bool allowZoom = true;
        public float zoomSmooth = 100;
        public float zoomStep = 2;
        public float maxZoom = -30;
        public float minZoom = -60;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float newDistance = -10; //used for smooth zooming - gives us a "destination" zoom
        [HideInInspector]
        public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        //holding our current x and y rotation for our camera
        //bool for allowing orbit
        public float xRotation = -65;
        public float yRotation = -180;
        public float maxYRotation = 200;
        public float minYRotation = -200;
        public bool allowOrbit = true;
        public float yOrbitSmooth = 0.5f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string MOUSE_ORBIT = "MouseOrbit";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    CollisionHandler collision;
    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero; 
    Vector3 camVelocity = Vector3.zero;
    Vector3 currentMousePosition = Vector3.zero;
    Vector3 previousMousePosition = Vector3.zero;
    float mouseOrbitInput, zoomInput;

    void Start()
    {
        //setting camera target
        SetCameraTarget(target);

        collision = GetComponent<CollisionHandler>();

        if (target)
        {
            MoveToTarget();
            collision.Initialize(Camera.main);
            collision.UpdateCollisionHandler(destination, targetPos);
        }
        position.newDistance = position.distanceFromTarget;
    }

    public void SetCameraTarget(Transform t)
    {
        //if we want to set a new target at runtime
        target = t;

        if (target == null)
        {
            Debug.LogError("Your camera needs a target");
        }
    }

    void GetInput()
    {
        //filling the values for our input variables
        mouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void Update()
    {
        //getting input 
        //zooming
        GetInput();
        if (position.allowZoom)
        {
            ZoomInOnTarget();
        }
    }

    void FixedUpdate()
    {
        //movetotarget
        //lookattarget
        //orbit
        if (target)
        {
            MoveToTarget();
            LookAtTarget();
            MouseOrbitTarget();

            collision.UpdateCollisionHandler(destination, targetPos);
            position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
        }
    }

    void MoveToTarget()
    {
        //handling getting our camera to its destination position
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x); 
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVelocity, position.smooth);
            }
            else
                transform.position = adjustedDestination;
        }
        else
        {
            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVelocity, position.smooth);
            }
            else
                transform.position = destination;
        }
    }

    void LookAtTarget()
    {
        //handling getting our camera to look at the target at all times
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = targetRotation;
    }

    void MouseOrbitTarget()
    {
        //getting the camera to orbit around our character
        previousMousePosition = currentMousePosition;
        currentMousePosition = Input.mousePosition;

        if (mouseOrbitInput > 0)
        {
            orbit.yRotation += (currentMousePosition.x - previousMousePosition.x) * orbit.yOrbitSmooth;

            if (orbit.yRotation > orbit.maxYRotation)
                orbit.yRotation = orbit.maxYRotation;
            if (orbit.yRotation < orbit.minYRotation)
                orbit.yRotation = orbit.minYRotation;

            UpdateYOrbitValue(orbit.yRotation);
        }
    }

    void ZoomInOnTarget()
    {
        //modifying the distancefromtarget to be closer or further away from our target
        position.newDistance += position.zoomStep * zoomInput;

        position.distanceFromTarget = Mathf.Lerp(position.distanceFromTarget, position.newDistance, position.zoomSmooth * Time.deltaTime);

        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
            position.newDistance = position.maxZoom;
        }
        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
            position.newDistance = position.minZoom;
        }
    }
}
