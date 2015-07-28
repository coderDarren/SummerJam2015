using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PickUpObject : MonoBehaviour {

    public bool childOfPickup = false;

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
        if (!childOfPickup)
        {
            pickUpObject.transform.localPosition = Vector3.Lerp(pickUpObject.transform.localPosition, (Vector3.up * 5 + Vector3.forward * 6), 100 * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        MagnetAction.PickUpMagnet += HoldObject;
        Magnet.DropObject += DropObject;
    }

    void OnDisable()
    {
        MagnetAction.PickUpMagnet -= HoldObject;
        Magnet.DropObject -= DropObject;
    }

    void HoldObject(GameObject obj)
    {
        pickUpObject = obj;
        obj.transform.parent = transform;
        holding = true;
    }

    void DropObject(GameObject obj)
    {
        pickUpObject.transform.parent = null;
        pickUpObject = null;
        holding = false;

        //in the case of holding a magnet
        transform.parent = null; //if the player drops a magnet they are child to
        childOfPickup = false;
        GetComponent<CharacterController>().underMagnetControl = false;
    }

}
