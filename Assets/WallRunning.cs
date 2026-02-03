using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	BasicMovement basicMovement;
	KeyCode wallRunKey = KeyCode.T;
	Coroutine wallRunTimerRoutine;
	[Header("Random ahh Variables")]
	[SerializeField] float maxWallRunDuration = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		basicMovement = GetComponent<BasicMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(wallRunKey) && !playerStats.isTouchingFloor && playerStats.isTouchingWall && !playerStats.isHanging && !playerStats.isWallRunning)
		{
			WallRun(true);
		}
		if (playerStats.isWallRunning && !playerStats.canMove)
		{
			basicMovement.ReadMovementInput(new KeyCode[] { KeyCode.W });
		}
		if (playerStats.isTouchingWall == false)
		{
			WallRun(false);
		}
    }

    public void WallRun(bool wallRunStatus)
    {
		Vector3 previousFacing = transform.forward;
		previousFacing.y = 0f;
		previousFacing.Normalize();

        if (wallRunStatus) playerStats.TeleportToRandomWall();
		playerStats.canMove = !wallRunStatus;
		playerStats.canLook = !wallRunStatus;
		playerStats.isWallRunning = wallRunStatus;
		rb.isKinematic = wallRunStatus;

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
				playerStats.FaceTowardsSpot(playerStats.interactedWall);
				playerStats.FaceTowardsSpot(transform.right);
				anim.SetFloat("WallRunningDirection", 1f);
			}
			else
			{
				// Player was facing towards the wall's backward direction (wall backward is to the player's left).
				playerStats.FaceTowardsSpot(playerStats.interactedWall);
				playerStats.FaceTowardsSpot(-transform.right);
				anim.SetFloat("WallRunningDirection", -1f);
			}
		}
		anim.SetBool("WallRunning", wallRunStatus);

		if (wallRunStatus) StartWallRunTimer();
		else if (wallRunTimerRoutine != null) StopCoroutine(wallRunTimerRoutine);
    }

	void StartWallRunTimer()
    {
		if (wallRunTimerRoutine != null) StopCoroutine(wallRunTimerRoutine);
		wallRunTimerRoutine = null;
        wallRunTimerRoutine = StartCoroutine(WallRunTimer());
    }

	IEnumerator WallRunTimer()
    {
        yield return new WaitForSeconds(maxWallRunDuration);
		WallRun(false);
    }
}
