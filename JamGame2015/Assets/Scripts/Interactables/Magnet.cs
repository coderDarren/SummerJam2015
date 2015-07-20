using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magnet : MonoBehaviour {

    public enum Charge {None, Positive, Negative };
    public Charge charge;
    public Color posChargeColor, negChargeColor, noChargeColor;
    public LayerMask magnetLayer;
    public float magnetLookUpDelay = 0.25f; //how often we add new magnets to the magnet array
    public float magnetStrength = 1;
    public float magnetRadius = 3;
    public float maxForceOnThis = 100;

    Rigidbody rBody;
    public Collider[] proximityMagnets;
    Vector3 totalForce = Vector3.zero;
    Vector3 newForce = Vector3.zero;
    Vector3 prevForce = Vector3.zero;
    Collider coll;
    Material lightMat;
    float delayTimer = 0;

    float slowTimeFactor = 1;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        lightMat = GetComponent<Renderer>().materials[1];
    }

    void Update()
    {
        if (charge != Charge.None)
        {
            GetMagnets();
            AddForceToMagnets();
            Debug.DrawRay(transform.position, totalForce, Color.cyan);
        }

        switch (charge)
        {
            case Charge.None:
                lightMat.color = noChargeColor;
                break;
            case Charge.Positive:
                lightMat.color = posChargeColor;
                break;
            case Charge.Negative:
                lightMat.color = negChargeColor;
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
