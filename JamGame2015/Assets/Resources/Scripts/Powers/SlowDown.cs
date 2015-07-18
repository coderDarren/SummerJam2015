﻿using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour {

    public delegate void TimeandSpace(float speedFactor);
    public static event TimeandSpace AlterSpeed;

    public string INPUT_AXIS = "SlowTime";
	public float speedFactor = 0.5f;
    public float duration = 5;
    public float cooldown = 10;

    public float durationTimer = 0;
    public float cooldownTimer = 0;
    bool activated = false;
    bool canActivate = true;

    void RunTimers()
    {
        if (activated)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer >= duration)
            {
                activated = false;
                cooldownTimer = 0; //start cooldown, the power was used completely
                AlterSpeed(1);
            }
        }
        else
        {
            if (cooldownTimer > cooldown)
            {
                canActivate = true;
            }
            else
            {
                cooldownTimer += Time.deltaTime;
                canActivate = false;
            }
        }
    }

    void Update () {
		if (Input.GetKeyUp(KeyCode.F)) {
            activated = !activated; //toggle activated
            if (activated)
            {
                if (canActivate) //if the cooldown time has been reached
                {
                    AlterSpeed(speedFactor);
                    durationTimer = 0; //reset duration, the power has been activated
                }
                else //cannot be actived if cooldown is still charging
                    activated = false;
            }
            else
            {
                AlterSpeed(1);
                cooldownTimer = 0; //start cooldown, the power was de-activated
            }
		}

        RunTimers();
	}
}
