using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float turnSpeed = 180f;

    [Header("Camera")]
    Transform cameraTransform;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float minPitch = -35f;
    [SerializeField] float maxPitch = 65f;

    Rigidbody rb;
    float pitch;
    Vector3 moveInput;
    float yawDelta;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ReadMovementInput();
        HandleLook();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void ReadMovementInput()
    {
        moveInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { moveInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { moveInput += Vector3.back; }
        if (Input.GetKey(KeyCode.A)) { moveInput += Vector3.left; }
        if (Input.GetKey(KeyCode.D)) { moveInput += Vector3.right; }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yawDelta += mouseX * turnSpeed * Time.deltaTime;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (cameraTransform != null)
        {
            Vector3 localEuler = cameraTransform.localEulerAngles;
            localEuler.x = pitch;
            cameraTransform.localEulerAngles = localEuler;
        }
		
		// TODO: Have a raycast between character's head and the camera to determine if there is anything
		// blocking the player's vision of the character
    }

    void HandleMovement()
    {
        if (moveInput.sqrMagnitude > 0f)
        {
            moveInput.Normalize();
            Vector3 worldMove = transform.TransformDirection(moveInput) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + worldMove);
        }

        if (Mathf.Abs(yawDelta) > 0f)
        {
            Quaternion yawRotation = Quaternion.Euler(0f, yawDelta, 0f);
            rb.MoveRotation(rb.rotation * yawRotation);
            yawDelta = 0f;
        }
    }

}
