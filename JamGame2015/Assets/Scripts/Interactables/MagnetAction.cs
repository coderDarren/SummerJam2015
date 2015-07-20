using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MagnetAction : MonoBehaviour, IPointerClickHandler {

    public delegate void ActionClickHandler(Magnet.Charge charge);
    public static event ActionClickHandler SetMagnetCharge;
    public delegate void CancelActionHandler();
    public static event CancelActionHandler CloseMagnetOptions;
    public delegate void PickUpHandler(GameObject go);
    public static event PickUpHandler PickUpMagnet;

    public enum ChargeSetter { Positive, Negative, None, Pickup, Cancel };
    public ChargeSetter chargeToSet;

    Magnet magnet;

    void Start()
    {
        magnet = GameObject.FindGameObjectWithTag("ToolUI").GetComponent<MagnetOptions>().magnet;
    }

    public void OnPointerClick(PointerEventData ped)
    {
        switch (chargeToSet)
        {
            case ChargeSetter.Positive: SetMagnetCharge(Magnet.Charge.Positive); break;
            case ChargeSetter.Negative: SetMagnetCharge(Magnet.Charge.Negative); break;
            case ChargeSetter.None: SetMagnetCharge(Magnet.Charge.None); break;
            case ChargeSetter.Pickup: PickUpMagnet(magnet.gameObject); CloseMagnetOptions(); break;
            case ChargeSetter.Cancel: CloseMagnetOptions(); break;
        }
    }
}
