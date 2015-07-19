using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CircularAttributeBar : MonoBehaviour {

    public enum BarType { Duration, Cooldown };
    public BarType barType;

	public GameObject wedge;
	public int numberOfWedges = 10;
	public float lowAngle = 0;
	public float highAngle = 90;


	RectTransform rt;
	float angle = 0; //how much rotation should be applied for the next wedge to be instantiated
	float angleRange; //how far around the grommet
	float rotationAngle = 0;
	List<GameObject> wedges;
	int numLiveWedges;

    float cooldownLength, cooldownTimer;
    float durationLength, durationTimer;

	void Start()
	{
		rt = GetComponent<RectTransform>();
		rotationAngle = -lowAngle;
		angleRange = highAngle - lowAngle;
		angle = angleRange / numberOfWedges;
		wedges = new List<GameObject>();
		numLiveWedges = numberOfWedges;

		SpawnWedges();
	}

	void SpawnWedges()
	{
		for (int i = 0; i < numberOfWedges; i++)
		{
			GameObject w = Instantiate(wedge as GameObject) as GameObject;
			RectTransform rTransform = w.GetComponent<RectTransform>();
			rTransform.SetParent(rt, false);
			rTransform.eulerAngles = new Vector3(0, 0, rotationAngle);

			wedges.Add(w);
			rotationAngle -= angle;
		}
	}

    void Update()
    {
        UpdateBar(cooldownTimer, cooldownLength);
        UpdateBar(durationTimer, durationLength);
    }


	void UpdateBar(float currValue, float maxValue)
	{
		if (currValue <= (maxValue/numberOfWedges) * (numLiveWedges-1))
		{
			if (numLiveWedges > 0) {
				numLiveWedges--;
				SubtractWedge();
			}
		}

		if (currValue >= (maxValue/numberOfWedges) * (numLiveWedges))
		{
			if (numLiveWedges < numberOfWedges) {
				numLiveWedges++;
				AddWedge();
			}
		}
	}

	void SubtractWedge()
	{
		wedges[numLiveWedges].SendMessage("ToggleFade");
	}

	void AddWedge()
	{
		wedges[numLiveWedges-1].SendMessage("ToggleFade");
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
        this.cooldownLength = cooldownLength;
        this.cooldownTimer = cooldownTimer;
    }

    void UpdateDuration(float durationLength, float durationTimer)
    {
        this.durationLength = durationLength;
        this.durationTimer = durationTimer;
    }
		
}
