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
        if (Input.GetKeyDown(preFwdAirKey) && playerStats.isTouchingWall && playerStats.HasWallInFront())
		{
			HangOnWall(true);
			
			// TODO:
			// - Make the camera cinematically move to a good position when the player is holding onto the wall, 
			// then we can let the player choose which direction to jump towards.
		}
    }
	
	public void HangOnWall(bool hangStatus = true)
	{
		if (hangStatus) playerStats.TeleportToRandomWall();
		playerStats.canMove = !hangStatus;
		//playerStats.canLook = !hangStatus;
		playerStats.isHanging = hangStatus;
		if (hangStatus) playerStats.FaceTowardsSpot(playerStats.interactedWall);
		anim.SetBool("Hanging", hangStatus);
		rb.isKinematic = hangStatus;
	}
}
