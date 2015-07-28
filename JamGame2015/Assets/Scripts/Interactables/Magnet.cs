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

    Rigidbody rBody;
    Collider[] proximityMagnets;
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
                lightMat.color = noChargeColor;
                magnetLight.color = noChargeColor;
                break;
            case Charge.Positive:
                lightMat.color = posChargeColor;
                magnetLight.color = posChargeColor;
                break;
            case Charge.Negative:
                lightMat.color = negChargeColor;
                magnetLight.color = negChargeColor;
                break;
        }
    }

	void FixedUpdate()
    {
        if (charge != Charge.None)
        {
            rBody.AddForce(totalForce * slowTimeFactor, ForceMode.Acceleration);
        }
    }

    void CheckForPlayerHoldingMagnet()
    {
        foreach(Collider magnet in proximityMagnets)
        {
            if (magnet.transform.parent == player && magnet.GetComponent<Magnet>().charge != Charge.None)
            {
                //swap parents - this way, if there is a magnetic pull on the magnet, the player will follow
                magnet.transform.parent = null;
                player.parent = magnet.transform;
                player.GetComponent<PickUpObject>().childOfPickup = true;
                //player.GetComponent<Rigidbody>().isKinematic = true;
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
                        Vector3 dir = magnet.coll.ClosestPointOnBounds(transform.position) - transform.position;
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
