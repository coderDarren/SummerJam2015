using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PickUpObject : MonoBehaviour {

    bool holding = false;
    GameObject pickUpObject;

    //animation variables
    public bool Holding() { return holding; }


    void Update()
    {
        if (Holding())
        {
            PickObjectUp();
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            if (pickUpObject)
                DropObject(pickUpObject);
        }
    }

    void PickObjectUp()
    {
        pickUpObject.transform.localPosition = Vector3.Lerp(pickUpObject.transform.localPosition, (Vector3.up * 2 + Vector3.forward * 8), 15 * Time.deltaTime);
    }

    void OnEnable()
    {
        MagnetAction.PickUpMagnet += HoldObject;
    }

    void OnDisable()
    {
        MagnetAction.PickUpMagnet -= HoldObject;
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
