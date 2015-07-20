﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MagnetAction : MonoBehaviour, IPointerClickHandler {

    public delegate void ActionClickHandler(Magnet.Charge charge);
    public static event ActionClickHandler SetMagnetCharge;

    public enum ChargeSetter { Positive, Negative, None };
    public ChargeSetter chargeToSet;

    public void OnPointerClick(PointerEventData ped)
    {
        switch (chargeToSet)
        {
            case ChargeSetter.Positive: SetMagnetCharge(Magnet.Charge.Positive); break;
            case ChargeSetter.Negative: SetMagnetCharge(Magnet.Charge.Negative); break;
            case ChargeSetter.None: SetMagnetCharge(Magnet.Charge.None); break;
        }
    }
}