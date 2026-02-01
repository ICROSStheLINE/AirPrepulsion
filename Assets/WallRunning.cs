using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	KeyCode wallRunKey = KeyCode.T;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(wallRunKey) && !playerStats.isTouchingFloor && playerStats.isTouchingWall && !playerStats.isHanging && !playerStats.isWallRunning)
		{
			WallRun(true);
		}
    }

    void WallRun(bool wallRunStatus)
    {
		Vector3 previousFacing = transform.forward;
		previousFacing.y = 0f;
		previousFacing.Normalize();

        if (wallRunStatus) playerStats.TeleportToRandomWall();
		playerStats.canMove = !wallRunStatus;
		playerStats.canLook = !wallRunStatus;
		playerStats.isWallRunning = wallRunStatus;
		if (wallRunStatus)
		{
			Vector3 wallForward = playerStats.interactedWall.transform.right;
			wallForward.y = 0f;
			wallForward.Normalize();

			float towardsWallForward = Vector3.Dot(previousFacing, wallForward);
			float towardsWallBackward = Vector3.Dot(previousFacing, -wallForward);

			if (towardsWallForward >= towardsWallBackward)
			{
				// Player was facing towards the wall's forward direction (wall forward is to the player's right).
				// TODO: Add your forward-direction wall-run behavior here.
				playerStats.FaceTowardsSpot(playerStats.interactedWall);
				playerStats.FaceTowardsSpot(transform.right);
				anim.SetFloat("WallRunningDirection", -1f);
			}
			else
			{
				// Player was facing towards the wall's backward direction (wall backward is to the player's left).
				// TODO: Add your backward-direction wall-run behavior here.
				playerStats.FaceTowardsSpot(playerStats.interactedWall);
				playerStats.FaceTowardsSpot(-transform.right);
				anim.SetFloat("WallRunningDirection", 1f);
			}
			// Face the player directly parallel with the wall
			// Maybe use the existing functions in PlayerStats like FaceTowardsSpot()
		}
		anim.SetBool("WallRunning", wallRunStatus);
		
		rb.isKinematic = wallRunStatus;
    }
}
