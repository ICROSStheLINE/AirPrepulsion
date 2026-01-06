using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging : MonoBehaviour
{
	Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	KeyCode preFwdAirKey = KeyCode.R;
	Transform cameraTransform;
	Coroutine cameraHangRoutine;
	
	[Header("GameObjects")]
    [SerializeField] GameObject cameraHangingPoint;
	[Header("Random ahh Variables")]
	[SerializeField] float cameraHangMoveSpeed = 8f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();

		if (cameraTransform == null && Camera.main != null)
		{
			cameraTransform = Camera.main.transform;
		}
    }

    void Update()
    {
        if (Input.GetKeyDown(preFwdAirKey) && playerStats.isTouchingWall && playerStats.HasWallInFront())
		{
			HangOnWall(true);

			StartCameraHangMove();
			
			// TODO: 
			// - Then we can let the player choose which direction to jump towards
		}
    }
	
	public void HangOnWall(bool hangStatus = true)
	{
		if (hangStatus) playerStats.TeleportToRandomWall();
		playerStats.canMove = !hangStatus;
		playerStats.canLook = !hangStatus;
		playerStats.canOcclusionCheck = !hangStatus;
		playerStats.isHanging = hangStatus;
		if (hangStatus) playerStats.FaceTowardsSpot(playerStats.interactedWall);
		anim.SetBool("Hanging", hangStatus);
		rb.isKinematic = hangStatus;

		if (!hangStatus && cameraHangRoutine != null)
		{
			StopCoroutine(cameraHangRoutine);
			cameraHangRoutine = null;
		}
	}

	void StartCameraHangMove()
	{
		if (cameraTransform == null || cameraHangingPoint == null)
		{
			return;
		}

		if (cameraHangRoutine != null)
		{
			StopCoroutine(cameraHangRoutine);
		}
		cameraHangRoutine = StartCoroutine(MoveCameraToHangPoint());
	}

	IEnumerator MoveCameraToHangPoint()
	{
		Transform target = cameraHangingPoint.transform;
		while (playerStats.isHanging)
		{
			Vector3 current = cameraTransform.position;
			Vector3 targetPos = target.position;
			Vector3 toTarget = targetPos - current;
			if (toTarget.sqrMagnitude <= 0.0004f)
			{
				cameraTransform.position = targetPos;
				break;
			}

			float t = 1f - Mathf.Exp(-cameraHangMoveSpeed * Time.deltaTime);
			cameraTransform.position = current + toTarget * t;
			yield return null;
		}

		cameraHangRoutine = null;
	}
}
