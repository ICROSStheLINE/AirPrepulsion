using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hanging : MonoBehaviour
{
	Rigidbody rb;
	Animator anim;
	PlayerStats playerStats;
	WallRunning wallRunning;
	KeyCode preFwdAirKey = KeyCode.R;
	Transform cameraTransform;
	Coroutine cameraHangRoutine;
	Coroutine hangTimerRoutine;
	Quaternion hangBaseRotation = Quaternion.identity;
	float hangYaw;
	float hangPitch;
	
	[Header("GameObjects")]
    [SerializeField] GameObject cameraHangingPoint;
	[Header("Random ahh Variables")]
	[SerializeField] float maxHangDuration = 2f;
	[SerializeField] float cameraHangMoveSpeed = 8f;
	[SerializeField] float hangLookSensitivity = 2f;
	float maxHangLookAngle = 60f;
	[Header("UI")]
	[SerializeField] GameObject hangTimerUI;
	[SerializeField] Image hangTimerFill;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		wallRunning = GetComponent<WallRunning>();

		if (cameraTransform == null && Camera.main != null)
		{
			cameraTransform = Camera.main.transform;
		}

		SetHangTimerUIActive(false);
    }

    void Update()
    {
		if (Input.GetKeyDown(preFwdAirKey) && playerStats.isWallRunning)
		{
			wallRunning.WallRun(false);
			HangOnWall(true, playerStats.interactedWall);
		}
        else if (Input.GetKeyDown(preFwdAirKey) && !playerStats.isTouchingFloor && playerStats.isTouchingWall && playerStats.HasWallInDirection(0f) && !playerStats.isHanging)
		{
			HangOnWall(true);
		}

		if (playerStats.isHanging)
		{
			HandleHangLook();
		}
    }
	
	public void HangOnWall(bool hangStatus)
	{
		if (hangStatus) playerStats.TeleportToRandomWall();
		playerStats.canMove = !hangStatus;
		playerStats.canLook = !hangStatus;
		playerStats.canOcclusionCheck = !hangStatus;
		playerStats.isHanging = hangStatus;
		//playerStats.EnableCrosshair(hangStatus);
		if (hangStatus) playerStats.FaceTowardsSpot(playerStats.interactedWall);
		anim.SetBool("Hanging", hangStatus);
		rb.isKinematic = hangStatus;

		if (!hangStatus && cameraHangRoutine != null)
		{
			StopCoroutine(cameraHangRoutine);
			cameraHangRoutine = null;
		}

		if (!hangStatus)
		{
			hangYaw = 0f;
			hangPitch = 0f;
		}
		StartCameraHangMove();
		if (hangStatus) StartHangTimer();
		else if (hangTimerRoutine != null) StopCoroutine(hangTimerRoutine);
		if (!hangStatus) SetHangTimerUIActive(false);
	}

	public void HangOnWall(bool hangStatus, GameObject wall)
	{
		if (hangStatus) playerStats.TeleportToWall(wall);
		playerStats.canMove = !hangStatus;
		playerStats.canLook = !hangStatus;
		playerStats.canOcclusionCheck = !hangStatus;
		playerStats.isHanging = hangStatus;
		//playerStats.EnableCrosshair(hangStatus);
		if (hangStatus) playerStats.FaceTowardsSpot(wall);
		anim.SetBool("Hanging", hangStatus);
		rb.isKinematic = hangStatus;

		if (!hangStatus && cameraHangRoutine != null)
		{
			StopCoroutine(cameraHangRoutine);
			cameraHangRoutine = null;
		}

		if (!hangStatus)
		{
			hangYaw = 0f;
			hangPitch = 0f;
		}
		StartCameraHangMove();
		if (hangStatus) StartHangTimer();
		else if (hangTimerRoutine != null) StopCoroutine(hangTimerRoutine);
		if (!hangStatus) SetHangTimerUIActive(false);
	}

	void StartHangTimer()
    {
		if (hangTimerRoutine != null) StopCoroutine(hangTimerRoutine);
		hangTimerRoutine = null;
        hangTimerRoutine = StartCoroutine(HangTimer());
    }

	IEnumerator HangTimer()
    {
		if (maxHangDuration <= 0f)
		{
			SetHangTimerUIActive(false);
			HangOnWall(false);
			yield break;
		}

		float elapsed = 0f;
		SetHangTimerUIActive(true);
		while (elapsed < maxHangDuration && playerStats.isHanging)
		{
			elapsed += Time.deltaTime;
			float remaining = Mathf.Clamp01(1f - (elapsed / maxHangDuration));
			if (hangTimerFill != null) hangTimerFill.fillAmount = remaining;
			yield return null;
		}

		SetHangTimerUIActive(false);
		if (playerStats.isHanging) HangOnWall(false);
    }

	void SetHangTimerUIActive(bool active)
	{
		if (hangTimerUI != null) hangTimerUI.SetActive(active);
		if (hangTimerFill != null) hangTimerFill.fillAmount = active ? 1f : 0f;
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
		hangBaseRotation = cameraHangingPoint.transform.rotation;
		hangYaw = 0f;
		hangPitch = 0f;
		cameraTransform.rotation = hangBaseRotation;
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

	void HandleHangLook()
	{
		if (cameraTransform == null || cameraHangingPoint == null)
		{
			return;
		}

		float mouseX = Input.GetAxis("Mouse X") * hangLookSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * hangLookSensitivity;

		hangYaw += mouseX;
		hangPitch -= mouseY;
		hangYaw = Mathf.Clamp(hangYaw, -maxHangLookAngle, maxHangLookAngle);
		hangPitch = Mathf.Clamp(hangPitch, -maxHangLookAngle, maxHangLookAngle);

		Quaternion desired = hangBaseRotation * Quaternion.Euler(hangPitch, hangYaw, 0f);
		cameraTransform.rotation = desired;
	}
}
