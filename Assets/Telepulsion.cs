using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telepulsion : MonoBehaviour
{
    // Rigidbody rb;
	// Animator anim;
	PlayerStats playerStats;
	Hanging hanging;
    KeyCode preFwdAirKey = KeyCode.R;

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
		// anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		hanging = GetComponent<Hanging>();
    }
    void Update()
    {
        if (Input.GetKeyUp(preFwdAirKey) && playerStats.isHanging && playerStats.isHoveringTelepulsionWall)
        {
            hanging.HangOnWall(false);
            playerStats.TeleportToWall(playerStats.hoveredTelepulsionWall);
            hanging.HangOnWall(true, playerStats.hoveredTelepulsionWall);
        }
    }
}
