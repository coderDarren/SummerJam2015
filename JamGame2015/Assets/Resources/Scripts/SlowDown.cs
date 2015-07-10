using UnityEngine;
using System.Collections;

public class SlowDown : MonoBehaviour {
	public float slowDownFactor;
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("e")) {
			if (Time.timeScale == 1) {
				Time.timeScale = slowDownFactor;
			}
			else {
				Time.timeScale = 1.0f;
			}
			Time.fixedDeltaTime = slowDownFactor;
		}
	}
}
