using UnityEngine;
using System.Collections;

public class SimpleLerper : MonoBehaviour {

	public Vector3 pointA, pointB;
	public bool parentable;
	public float speed;

	// Use this for initialization
	void Start () {
		pointA = this.transform.position;
	}

	IEnumerator MoveBlock(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i= 0.0f;
		var rate= 1.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == pointA)
		{
			StartCoroutine(MoveBlock(transform, pointA, pointB, speed));
		}
		else if (transform.position == pointB)
		{
			StartCoroutine(MoveBlock(transform, pointB, pointA, speed));
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag.Equals("Player"))
		{
			if (parentable)
				col.transform.parent = transform;
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag.Equals("Player"))
		{
			if (parentable)
				col.transform.parent = null;
		}
	}
}
