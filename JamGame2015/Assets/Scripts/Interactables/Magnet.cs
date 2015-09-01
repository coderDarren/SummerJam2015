using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magnet : MonoBehaviour {

    public delegate void PlayerPickUpHandler(GameObject obj);
    public static event PlayerPickUpHandler DropObject;

    public enum Charge {None, Positive, Negative };
    public Charge charge;
    public Color posChargeColor, negChargeColor, noChargeColor;
    public LayerMask magnetLayer;
    public float magnetLookUpDelay = 0.25f; //how often we add new magnets to the magnet array
    public float magnetStrength = 1;
    public float magnetRadius = 3;
    public float maxForceOnThis = 100;
    public bool attractToMagnetCenter = false;

    Rigidbody rBody;
    public Collider[] proximityMagnets;
    Vector3 totalForce = Vector3.zero;
    Vector3 newForce = Vector3.zero;
    Vector3 prevForce = Vector3.zero;
    Collider coll;
    Material lightMat;
    Light magnetLight;
    float delayTimer = 0;
    float slowTimeFactor = 1;

    Transform player;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        lightMat = GetComponent<Renderer>().materials[1];
        magnetLight = GetComponentInChildren<Light>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (charge != Charge.None)
        {
            GetMagnets();
            CheckForPlayerHoldingMagnet();
            AddForceToMagnets();
        }

        switch (charge)
        {
            case Charge.None:
                lightMat.SetColor("_EmissionColor", noChargeColor);
                lightMat.SetColor("_Color", noChargeColor);
                magnetLight.color = noChargeColor;
                gameObject.layer = LayerMask.NameToLayer("Default");
                break;
            case Charge.Positive:
                lightMat.SetColor("_EmissionColor", posChargeColor);
                lightMat.SetColor("_Color", posChargeColor);
                magnetLight.color = posChargeColor;
                gameObject.layer = LayerMask.NameToLayer("Magnet");
                break;
            case Charge.Negative:
                lightMat.SetColor("_EmissionColor", negChargeColor);
                lightMat.SetColor("_Color", negChargeColor);
                magnetLight.color = negChargeColor;
                gameObject.layer = LayerMask.NameToLayer("Magnet");
                break;
        }

        if (proximityMagnets.Length > 1 && charge != Charge.None)
            rBody.useGravity = false;
        else
            rBody.useGravity = true;
    }

	void FixedUpdate()
    {
        if (charge != Charge.None)
        {
            rBody.AddForce(totalForce * slowTimeFactor, ForceMode.Force);
            Debug.DrawLine(transform.position, transform.position + totalForce, Color.cyan);
        }
    }

    void CheckForPlayerHoldingMagnet()
    {
        foreach(Collider magnet in proximityMagnets)
        {
            if (magnet.transform.parent == player && magnet.GetComponent<Magnet>().charge != Charge.None && Mathf.Abs(magnet.GetComponent<Magnet>().totalForce.magnitude) > 1) //if proxMagnets length is greater than 1 it means there are magnets other than this applying force
            {
                //swap parents - this way, if there is a magnetic pull on the magnet, the player will follow
                magnet.transform.parent = null;
                player.parent = magnet.transform;
                player.GetComponent<PickUpObject>().childOfPickup = true;
                player.GetComponent<CharacterController>().underMagnetControl = true;
                player.GetComponent<CharacterController>().magnetInControl = magnet.gameObject;
                player.GetComponent<CharacterController>().magnetOffset = Vector3.Normalize(new Vector3(player.localPosition.x, player.localPosition.z, player.localPosition.y)) * 2;
                Quaternion targetRotation = Quaternion.LookRotation(magnet.transform.position - player.position, Vector3.up);
                player.eulerAngles = new Vector3(player.eulerAngles.x, targetRotation.eulerAngles.y, player.eulerAngles.z);
            }
            if (player.parent == magnet.transform && magnet.GetComponent<Magnet>().charge == Charge.None)
            {
                DropObject(magnet.gameObject); //PickUpObject.cs is listening
            }
        }
    }

    void GetMagnets()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > magnetLookUpDelay)
        {
            proximityMagnets = Physics.OverlapSphere(transform.position, magnetRadius, magnetLayer);
            delayTimer = 0;
        }
    }

    void AddForceToMagnets()
    {
        prevForce = totalForce;
        totalForce = Vector3.zero;
        int thisCharge = (int)charge;
        int otherCharge = 0;

        if (proximityMagnets != null)
        {
            for (int i = 0; i < proximityMagnets.Length; i++)
            {
                Magnet magnet = proximityMagnets[i].GetComponent<Magnet>();
                if (magnet != this)
                {
                    if (magnet.charge != Charge.None)
                    {
                        Vector3 dir;
                        if (magnet.attractToMagnetCenter)
                            dir = magnet.transform.position - transform.position;
                        else
                            dir = magnet.coll.ClosestPointOnBounds(transform.position) - transform.position;
                        
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, dir, out hit, magnetLayer))
                        {
                            otherCharge = (int)magnet.charge;
                            if (otherCharge == thisCharge)
                                newForce = transform.position - hit.point;
                            else
                                newForce = hit.point - transform.position;
                            newForce = Vector3.Normalize(newForce);
                            newForce = (newForce * magnet.magnetStrength) / rBody.mass;
                            totalForce += newForce;
                        }
                        
                        if (totalForce.magnitude > maxForceOnThis)
                        {
                            totalForce = prevForce;
                        }
                    }
                }

            }

        }

    }

    Vector3 ClosestPointOnMesh(Transform magnet)
    {
        Vector3 closest = Vector3.zero;
        Vector3[] vertices;
        Vector3 point = transform.InverseTransformDirection(transform.position);
        float minDistance = Mathf.Infinity;
        Mesh m = magnet.GetComponent<MeshFilter>().mesh;

        if (m)
            vertices = m.vertices;
        else
        {
            Debug.LogError("The mesh you are checking at is null or does not exist.");
            return closest;
        }

        if (vertices.Length > 0)
            closest = vertices[0];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 distance = point-vertices[i];
            float distSqrd = distance.sqrMagnitude;
            
            if (distSqrd < minDistance)
            {
                minDistance = distSqrd;
                closest = vertices[i];
            }
        }
        
        Debug.Log("Closest point: " + magnet.TransformPoint(closest));
        return magnet.TransformPoint(closest);
    }

    void OnEnable()
    {
        SlowDown.AlterSpeed += AlterSpeed;
    }

    void OnDisable()
    {
        SlowDown.AlterSpeed -= AlterSpeed;
    }

    void AlterSpeed(float speedFactor)
    {
        slowTimeFactor = speedFactor;
    }
}
