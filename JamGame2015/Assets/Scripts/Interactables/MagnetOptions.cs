﻿using UnityEngine;
using System.Collections;

public class MagnetOptions : MonoBehaviour {

    public delegate void MagnetInteractableHandler(bool canInteract);
    public static event MagnetInteractableHandler SetInteractability;

    public GameObject OptionsMenu;

    GameObject menu;
    public Magnet magnet;

    #region Magnet Manip

    void SetMagnetCharge(Magnet.Charge charge)
    {
        magnet.charge = charge;
        CloseMagnetOptions();
    }

    #endregion


    #region UI

    void OnEnable()
    {
        InteractiveCursor.OpenMagnetOptions += OpenMagnetOptions;
        InteractiveCursor.CloseMagnetOptions += CloseMagnetOptions;
        MagnetAction.SetMagnetCharge += SetMagnetCharge;
        MagnetAction.CloseMagnetOptions += CloseMagnetOptions;
    }

    void OnDisable()
    {
        InteractiveCursor.OpenMagnetOptions -= OpenMagnetOptions;
        InteractiveCursor.CloseMagnetOptions -= CloseMagnetOptions;
        MagnetAction.SetMagnetCharge -= SetMagnetCharge;
        MagnetAction.CloseMagnetOptions -= CloseMagnetOptions;
    }

    void OpenMagnetOptions(GameObject forMagnet)
    {
        if (menu)
            Destroy(menu);
        menu = Instantiate(OptionsMenu, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
        magnet = forMagnet.GetComponent<Magnet>();
        SetInteractability(false);
    }

    void CloseMagnetOptions()
    {
        Destroy(menu);
        InteractiveCursor.InteractionObject = null;
        SetInteractability(true);
    }

    #endregion
}
