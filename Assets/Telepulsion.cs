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
    // TODO: I think I should try making a level to see if I can make any interesting design
    // choices off of what currently exists... 
    // Maybe I can add some kind of extra ability? At this point if there's nothing left to 
    // add I'd have to start making some basic ahh enemies or targets or SOMETHING for the 
    // player to target when jumping from wall to wall.
    // MAYBE WALL RUNNING!! ADDING WALL RUNNING WOULD BE SICK! That way the player has some 
    // movement options before jumping off the wall. Wall running shouldn't be possible on 
    // telepulsion platforms.
    // When a player lands on a normal wall they shouldn't immediately start hanging. Instead 
    // they should start running on the wall somehow. When the prepulsion key is pressed they'd 
    // hang and prepare to launch, then as usual when it's released they'd propel.
    // Holy genius
    void Update()
    {
        if (Input.GetKeyUp(preFwdAirKey) && playerStats.isHanging && playerStats.isHoveringTelepulsionWall)
        {
            TelepulsionToWall();
        }
        else if (Input.GetKeyUp(preFwdAirKey) && playerStats.isHoveringTelepulsionWall)
        {
            TelepulsionToWall();
        }
    }
    
    void TelepulsionToWall()
    {
        hanging.HangOnWall(false);
        hanging.HangOnWall(true, playerStats.hoveredTelepulsionWall);
        playerStats.TeleportToWallCentre(playerStats.hoveredTelepulsionWall);
        playerStats.FaceTowardsSpot(playerStats.hoveredTelepulsionWall);
    }
}
