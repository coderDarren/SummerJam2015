using UnityEngine;
using System.Collections;

public class TeleportInteraction : MonoBehaviour {

    public bool canInteract = false;
    Ray mouseRay;
    RaycastHit hitInfo;

    void FixedUpdate()
    {
        if (canInteract)
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hitInfo))
            {
                if (hitInfo.collider.tag.Equals("Teleporter"))
                {
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        Teleporter tp = hitInfo.collider.GetComponent<Teleporter>();
                        tp.Teleport(gameObject);
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Teleporter"))
            canInteract = true;
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag.Equals("Teleporter"))
            canInteract = false;
    }
}
