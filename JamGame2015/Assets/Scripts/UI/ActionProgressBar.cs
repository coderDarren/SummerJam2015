using UnityEngine;
using System.Collections;

public class ActionProgressBar : MonoBehaviour {

    public enum BarType { Duration, Cooldown };
    public BarType barType;

    RectTransform rt;
    Vector3 scale;
    Vector3 initialScale;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        scale = rt.localScale;
        initialScale = scale;
    }

    void UpdateBar(float max, float current)
    {
        scale.x = (initialScale.x * current) / max;

        if (scale.x > 1)
            scale.x = initialScale.x;
        if (scale.x < 0)
            scale.x = 0;

        rt.localScale = scale;
    }

    void OnEnable()
    {
        switch (barType)
        {
            case BarType.Cooldown:
                SlowDown.UpdateCooldown += UpdateCooldown;
                break;
            case BarType.Duration:
                SlowDown.UpdateDuration += UpdateDuration;
                break;
        }
    }

    void OnDisable()
    {
        switch (barType)
        {
            case BarType.Cooldown:
                SlowDown.UpdateCooldown -= UpdateCooldown;
                break;
            case BarType.Duration:
                SlowDown.UpdateDuration -= UpdateDuration;
                break;
        }
    }

    void UpdateCooldown(float cooldownLength, float cooldownTimer)
    {
        UpdateBar(cooldownLength, cooldownTimer);
    }

    void UpdateDuration(float durationLength, float durationTimer)
    {
        UpdateBar(durationLength, durationTimer);
    }

}
