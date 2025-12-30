using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	Animator anim;
	
    [HideInInspector] public bool isTouchingFloor = true;
	[HideInInspector] public bool isTouchingWall = false;
	[HideInInspector] public readonly HashSet<GameObject> touchingWalls = new HashSet<GameObject>();

	
	
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			touchingWalls.Add(collision.gameObject);
			isTouchingWall = true;
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			touchingWalls.Remove(collision.gameObject);
			isTouchingWall = false;
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Floor")
		{
			isTouchingFloor = true;
			anim.SetBool("FallingIdle", false);
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "Floor")
		{
			isTouchingFloor = false;
			anim.SetBool("FallingIdle", true);
		}
	}
	
	public bool HasWallBehind(float maxAngle = 90f, float maxDistance = 2f)
	{
		Vector3 behind = -transform.forward;
		behind.y = 0f;
		behind.Normalize();

		foreach (GameObject wall in touchingWalls)
		{
			if (wall == null)
			{
				continue;
			}

			Vector3 toWall = wall.transform.position - transform.position;
			toWall.y = 0f;
			if (toWall.sqrMagnitude == 0f)
			{
				continue;
			}

			float angle = Vector3.Angle(behind, toWall);
			if (angle <= maxAngle)
			{
				if (maxDistance <= 0f || toWall.sqrMagnitude <= maxDistance * maxDistance)
				{
					return true;
				}
			}
		}

		return false;
	}
}
