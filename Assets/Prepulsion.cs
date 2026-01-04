using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepulsion : MonoBehaviour
{
	Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	KeyCode preUpKey = KeyCode.E;
	KeyCode preFwdAirKey = KeyCode.R;
	
	GameObject kickedWall = null;
	Vector3 awayFromWall;
	bool isHanging = false;
	
	static readonly float preUpAnimationDurationMultiplier = 1.2f;
	static readonly float preUpAnimationDuration = 0.458f / preUpAnimationDurationMultiplier;
	
	static readonly float preFwdAirAnimationDurationMultiplier = 1f;
	static readonly float preFwdAirAnimationDuration = 0.567f / preFwdAirAnimationDurationMultiplier;
	

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
    }


    void Update()
    {
		if (Input.GetKeyDown(preFwdAirKey) && playerStats.isTouchingWall && playerStats.HasWallBehind())
		{
			HangOnWall();
		}
		if (Input.GetKeyUp(preFwdAirKey) && isHanging)
		{
			// TODO: 
			// - Make it so that holding down the preFwdAirKey key makes the player character hold onto the wall instead of immediately kicking off of it.
			// - Letting go of the preFwdAirKey key should make the player character kick off of it as usual
			// - Once camera movement around walls is handled we can make the camera cinematically move to a good position when the player is holding onto 
			// the wall, then we can let the player choose which direction to jump towards
			isHanging = false;
			anim.SetBool("Hanging", false);
			StartCoroutine("PropelForwardAir");
		}
        if (Input.GetKeyDown(preUpKey) && playerStats.isTouchingFloor)
		{
			StartCoroutine("PropelUpwards");
		}
    }
	
	void HangOnWall()
	{
		TeleportToRandomWall();
		isHanging = true;
		FaceTowardsSpot(kickedWall);
		anim.SetBool("Hanging", true);
	}
	
	IEnumerator PropelForwardAir()
	{
		TeleportToRandomWall();

		anim.SetBool("AirKickFwd", true);
		yield return new WaitForSeconds(preFwdAirAnimationDuration/4);
		rb.linearVelocity = (awayFromWall * 10f) + (Vector3.up * 4f);
		anim.SetBool("AirKickFwd", false);
	}
	
	IEnumerator PropelUpwards()
	{
		anim.SetBool("ShootDown", true);
		yield return new WaitForSeconds(preUpAnimationDuration - 0.05f);
		anim.SetBool("ShootDown", false);
		rb.linearVelocity = new Vector3(0,10f,0);
		anim.SetBool("FallingIdle", true);
	}
	
	void TeleportToRandomWall() 
	{
		kickedWall = null;
		foreach (GameObject wall in playerStats.touchingWalls)
		{
			if (wall == null) continue;
			kickedWall = wall;
			break;
		}

		if (kickedWall == null)
		{
			return;
		}

		Collider wallCollider = kickedWall.GetComponent<Collider>();
		Vector3 closestPoint = wallCollider.ClosestPoint(transform.position);
		awayFromWall = transform.position - closestPoint;
		awayFromWall.y = 0f;
		FaceTowardsSpot(awayFromWall);
		
		const float wallClearance = 4.3f;
		transform.position = closestPoint + awayFromWall * wallClearance; // TODO: Make this line teleport the player a fixed distance away from the wall. At the moment the player's position has a slight influence on the awayFromWall variable. Figure out a way to solve that.
	}
	
	void FaceTowardsSpot(Vector3 spot)
	{
		spot.Normalize();
		transform.rotation = Quaternion.LookRotation(spot, Vector3.up);
	}
	
	void FaceTowardsSpot(GameObject objectWithCollider)
	{
		Vector3 towardsObject;
		Collider objectCollider = objectWithCollider.GetComponent<Collider>();
		Vector3 closestPoint = objectCollider.ClosestPoint(transform.position);
		towardsObject = closestPoint - transform.position;
		towardsObject.y = 0f;
		towardsObject.Normalize();
		transform.rotation = Quaternion.LookRotation(towardsObject, Vector3.up);
	}
}
