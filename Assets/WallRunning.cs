using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	KeyCode preUpKey = KeyCode.E;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(preUpKey) && !playerStats.isTouchingFloor && playerStats.isTouchingWall && playerStats.HasWallInDirection(0f) && !playerStats.isHanging)
		{
			BeginWallRun();
		}
    }

    void BeginWallRun()
    {
        
    }
}
