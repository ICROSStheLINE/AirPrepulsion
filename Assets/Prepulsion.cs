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

	float forwardVelocity = 5f;
	float upwardsVelocity = 8f;

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
		if (Input.GetKeyUp(preFwdAirKey) && playerStats.isHanging && !playerStats.isHoveringTelepulsionWall)
		{
			Vector3 launchForward = GetLaunchForward();
			hanging.HangOnWall(false);
			StartCoroutine(PropelForwardAir(launchForward));
		}
        if (Input.GetKeyDown(preUpKey) && playerStats.isTouchingFloor)
		{
			StartCoroutine("PropelUpwards");
		}
    }

	Vector3 GetLaunchForward()
	{
		Vector3 cameraForward = Camera.main.transform.forward;
		cameraForward.y = 0f;

		const float rightYawDegrees = 0f;
		Quaternion rightYaw = Quaternion.AngleAxis(rightYawDegrees, Vector3.up);
		return (rightYaw * cameraForward).normalized;
	}

	IEnumerator PropelForwardAir(Vector3 launchForward)
	{
		//playerStats.TeleportToRandomWall();

		anim.SetBool("AirKickFwd", true);
		Quaternion targetRotation = Quaternion.LookRotation(launchForward, Vector3.up);
		rb.MoveRotation(targetRotation);
		yield return new WaitForSeconds(preFwdAirAnimationDuration/4);
		rb.linearVelocity = (launchForward * forwardVelocity) + (Vector3.up * upwardsVelocity);
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
