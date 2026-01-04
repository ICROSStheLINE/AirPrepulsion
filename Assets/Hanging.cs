using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging : MonoBehaviour
{
	Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	KeyCode preFwdAirKey = KeyCode.R;

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
			HangOnWall(true);
		}
    }
	
	public void HangOnWall(bool hangStatus = true)
	{
		playerStats.TeleportToRandomWall();
		playerStats.canMove = !hangStatus;
		//playerStats.canLook = !hangStatus;
		playerStats.isHanging = hangStatus;
		playerStats.FaceTowardsSpot(playerStats.interactedWall);
		anim.SetBool("Hanging", hangStatus);
		rb.isKinematic = hangStatus;
	}
}
