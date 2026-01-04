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

    void Update()
    {

    }

	public void ReceivedContact(string surfaceTag, Collider surfaceCollision)
	{
		if (surfaceTag == "Floor")
		{
			isTouchingFloor = true;
			anim.SetBool("FallingIdle", false);
		}
		if (surfaceTag == "Wall")
		{
			touchingWalls.Add(surfaceCollision.gameObject);
			isTouchingWall = true;
		}
	}

	public void ExitContact(string surfaceTag, Collider surfaceCollision)
	{
		if (surfaceTag == "Floor")
		{
			isTouchingFloor = false;
			anim.SetBool("FallingIdle", true);
		}
		if (surfaceTag == "Wall")
		{
			touchingWalls.Remove(surfaceCollision.gameObject);
			isTouchingWall = false;
		}
	}
	
	
	
	public bool HasWallBehind(float maxAngle = 90f, float maxDistance = 40f)
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
