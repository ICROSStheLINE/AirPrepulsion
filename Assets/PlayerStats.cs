using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	Animator anim;

	[HideInInspector] public bool canMove = true;
	[HideInInspector] public bool canLook = true;

    [HideInInspector] public bool isTouchingFloor = true;
	[HideInInspector] public bool isTouchingWall = false;
	[HideInInspector] public bool isHanging = false;
	[HideInInspector] public readonly HashSet<GameObject> touchingWalls = new HashSet<GameObject>();
	[HideInInspector] public GameObject interactedWall = null;


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
	
	public void TeleportToRandomWall() 
	{
		Vector3 awayFromWall;
		interactedWall = null;
		foreach (GameObject wall in touchingWalls)
		{
			if (wall == null) continue;
			interactedWall = wall;
			break;
		}

		if (interactedWall == null)
		{
			return;
		}

		Collider wallCollider = interactedWall.GetComponent<Collider>();
		Vector3 closestPoint = wallCollider.ClosestPoint(transform.position);
		awayFromWall = transform.position - closestPoint;
		awayFromWall.y = 0f;
		awayFromWall = awayFromWall.normalized;
		FaceTowardsSpot(awayFromWall);

		const float wallClearance = 0.4f;
		transform.position = closestPoint + awayFromWall * wallClearance;
	}

	public void FaceTowardsSpot(Vector3 spot)
	{
		spot.Normalize();
		transform.rotation = Quaternion.LookRotation(spot, Vector3.up);
	}

	public void FaceTowardsSpot(GameObject objectWithCollider)
	{
		Vector3 towardsObject;
		Collider objectCollider = objectWithCollider.GetComponent<Collider>();
		Vector3 closestPoint = objectCollider.ClosestPoint(transform.position);
		towardsObject = closestPoint - transform.position;
		towardsObject.y = 0f;
		towardsObject.Normalize();
		transform.rotation = Quaternion.LookRotation(towardsObject, Vector3.up);
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
