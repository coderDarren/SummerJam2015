using UnityEngine;
using System.Collections;

public class Lerper : MonoBehaviour {

	public Vector3 pointA;
	public Vector3 pointB;
	public float speed;
	public float moveDist;
	public bool parentable = false;
	public bool moveX, moveY, moveZ;
	private Vector3 collBounds; 
	// Use this for initialization
	void Start () {
		pointA = this.transform.position;
		pointB = pointA;
		collBounds = gameObject.GetComponent<Collider>().bounds.size;
		if (moveX)
		{
			pointB = new Vector3 (transform.position.x + moveDist, transform.position.y, transform.position.z);
			if (moveDist > 0)
			{
				Ray ray = new Ray(transform.position, transform.right);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.x < pointB.x)
					{
						pointB.x = hit.transform.position.x - collBounds.x;
					}
				}
			}
			else
			{
				Ray ray = new Ray(transform.position, -transform.right);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.x > pointB.x)
					{
						pointB.x = hit.transform.position.x + collBounds.x;
					}
				}
			}
		}
		if (moveY)
		{
			pointB = new Vector3 (transform.position.x, transform.position.y + moveDist, transform.position.z);
			if (moveDist > 0)
			{
				Ray ray = new Ray(transform.position, transform.up);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.y < pointB.y)
					{
						pointB.y = hit.transform.position.y - collBounds.y;
					}
				}
			}
			else
			{
				Ray ray = new Ray(transform.position, -transform.up);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.y > pointB.y)
					{
						pointB.y = hit.transform.position.y + collBounds.y;
					}
				}
			}
		}
		if (moveZ)
		{
			pointB = new Vector3 (transform.position.x, transform.position.y, transform.position.z + moveDist);
			if (moveDist > 0)
			{
				Ray ray = new Ray(transform.position, transform.forward);
				RaycastHit hit;
				Debug.DrawRay(transform.position, -transform.forward);
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.z < pointB.z)
					{
						pointB.z = hit.transform.position.z - collBounds.z;
					}
				}
			}
			else
			{
				Ray ray = new Ray(transform.position, -transform.forward);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.position.z > pointB.z)
					{
						pointB.z = hit.transform.position.z + collBounds.z;
					}
				}
			}
		}
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
		Debug.DrawRay(transform.position, transform.forward);
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