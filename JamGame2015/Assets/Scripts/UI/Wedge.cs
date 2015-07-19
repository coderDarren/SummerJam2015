using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wedge : MonoBehaviour {

	public float fadeStep = 0.01f;
	public float minAlpha = 0.2f;

	Image img;
	Color wedgeColor;
	bool fadingOut = false;


	void Start()
	{
		img = GetComponent<Image>();
		wedgeColor = img.color;
	}

	void Update()
	{
		if (fadingOut)
		{
			if (wedgeColor.a > minAlpha)
			{
				Fade(minAlpha);
			}
		}
		else
		{
			if (wedgeColor.a < 1)
			{
				Fade(1);
			}
		}
	}

	public void ToggleFade()
	{
		fadingOut = !fadingOut;
	}

	void Fade(float target)
	{
		wedgeColor.a = Mathf.Lerp(wedgeColor.a, target, fadeStep * Time.deltaTime);
		img.color = wedgeColor;
	}
}
