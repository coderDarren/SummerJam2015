using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour {

    public delegate void TimeandSpace(float speedFactor);
    public static event TimeandSpace AlterSpeed;
    public delegate void ProgressBar(float max, float current);
    public static event ProgressBar UpdateCooldown;
    public static event ProgressBar UpdateDuration;
    public delegate void Activation(bool activated);
    public static event Activation SlowDownActive;

    public string INPUT_AXIS = "SlowTime";
	public float speedFactor = 0.5f;
    public float duration = 5;
    public float cooldown = 10;

    public float durationTimer = 0;
    public float cooldownTimer = 0;
    bool activated = false;
    bool canActivate = true;
    Vector3 _gravity; //this will be set to whatever the initial gravity setting is

    void Start()
    {
        _gravity = Physics.gravity;
    }

    void RunTimers()
    {
        if (activated)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer >= duration)
            {
                activated = false;
                cooldownTimer = 0; //start cooldown, the power was used completely
                durationTimer = 0.1f;
                AlterSpeed(1);
                SlowDownActive(false);
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
                    Physics.gravity *= speedFactor;
                    durationTimer = 0; //reset duration, the power has been activated
                    cooldownTimer = 0.05f;
                    SlowDownActive(true);
                }
                else //cannot be actived if cooldown is still charging
                    activated = false;
            }
            else
            {
                AlterSpeed(1);
                Physics.gravity = _gravity;
                cooldownTimer = 0; //start cooldown, the power was de-activated
                durationTimer = 0.1f;
                SlowDownActive(false);
            }
		}

        RunTimers();
        UpdateDuration(duration, durationTimer);
        UpdateCooldown(cooldown, cooldownTimer);
	}
}
