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
			// TODO: 
			// - Make the condition for holding onto the wall NOT be if a wall is behind a player, instead make
			// it so that the player can hold onto the wall IN FRONT of the player.
			
			// TODO:
			// - Make the camera cinematically move to a good position when the player is holding onto the wall, 
			// then we can let the player choose which direction to jump towards.
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
