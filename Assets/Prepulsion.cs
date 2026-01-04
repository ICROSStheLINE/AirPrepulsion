using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepulsion : MonoBehaviour
{
	Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	Hanging hanging;
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
		hanging = GetComponent<Hanging>();
    }

    void Update()
    {
		if (Input.GetKeyUp(preFwdAirKey) && playerStats.isHanging)
		{
			// TODO: 
			// - Make it so that holding down the preFwdAirKey key makes the player character hold onto the wall instead of immediately kicking off of it.
			// - Letting go of the preFwdAirKey key should make the player character kick off of it as usual
			// - Once camera movement around walls is handled we can make the camera cinematically move to a good position when the player is holding onto 
			// the wall, then we can let the player choose which direction to jump towards
			
			hanging.HangOnWall(false);
			StartCoroutine("PropelForwardAir");
		}
        if (Input.GetKeyDown(preUpKey) && playerStats.isTouchingFloor)
		{
			StartCoroutine("PropelUpwards");
		}
    }

	IEnumerator PropelForwardAir()
	{
		playerStats.TeleportToRandomWall();

		anim.SetBool("AirKickFwd", true);
		yield return new WaitForSeconds(preFwdAirAnimationDuration/4);
		rb.linearVelocity = (transform.forward * 10f) + (Vector3.up * 4f);
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
