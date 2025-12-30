using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float turnSpeed = 180f;

    [Header("Camera")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float minPitch = -35f;
    [SerializeField] float maxPitch = 65f;

    float pitch;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector3 moveInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { moveInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { moveInput += Vector3.back; }
        if (Input.GetKey(KeyCode.A)) { moveInput += Vector3.left; }
        if (Input.GetKey(KeyCode.D)) { moveInput += Vector3.right; }

        if (moveInput.sqrMagnitude > 0f)
        {
            moveInput.Normalize();
            Vector3 worldMove = transform.TransformDirection(moveInput) * moveSpeed * Time.deltaTime;
            transform.position += worldMove;
        }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up, mouseX * turnSpeed * Time.deltaTime);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (cameraTransform != null)
        {
            Vector3 localEuler = cameraTransform.localEulerAngles;
            localEuler.x = pitch;
            cameraTransform.localEulerAngles = localEuler;
        }
    }
}
