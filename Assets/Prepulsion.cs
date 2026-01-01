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
			StartCoroutine("PropelForwardAir");
		}
        if (Input.GetKeyDown(preUpKey) && playerStats.isTouchingFloor)
		{
			StartCoroutine("PropelUpwards");
		}
    }
	
	IEnumerator PropelForwardAir()
	{
		// TODO: Make WallDetection and FloorDetection objects functional and make INVISIBLE map geometry
		// that would be the only interactable objects when handling mesh geometry
		// In other words, mesh environments should have an invisible wall behind every object that would be 
		// the only thing the player should be able to kick off of.
		
		anim.SetBool("AirKickFwd", true);
		yield return new WaitForSeconds(preFwdAirAnimationDuration/2);
		rb.linearVelocity = new Vector3(0,0,0) + (transform.forward * 10) + (Vector3.up * 4);
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
}
