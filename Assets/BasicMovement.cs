using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float turnSpeed = 180f;

    [Header("Camera")]
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float minPitch = -35f;
    [SerializeField] float maxPitch = 65f;

    [Header("GameObjects")]
    [SerializeField] GameObject cameraReturnPoint;

    Rigidbody rb;
    Transform cameraTransform;
    float pitch;
    Vector3 moveInput;
    float yawDelta;
    bool isCameraOccluded;

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
        CheckCameraOcclusion();
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

    void CheckCameraOcclusion()
    {
        if (cameraTransform == null || cameraReturnPoint == null) // If there is no camera don't do ts LOL
        {
            isCameraOccluded = false;
            return;
        }

        Vector3 headPosition = transform.position + Vector3.up;
        Vector3 targetPosition = cameraReturnPoint.transform.position;
        Vector3 toTarget = targetPosition - headPosition;
        float distance = toTarget.magnitude;
        if (distance <= 0.001f)
        {
            isCameraOccluded = false;
            return;
        }
        Vector3 direction = toTarget / distance;
        
        RaycastHit hit;
        int playerMask = LayerMask.GetMask("Player"); // Player layer mask
        int layerMask = Physics.AllLayers ^ playerMask; // All layers EXCEPT THE player layer
        isCameraOccluded = Physics.Raycast( // Raycast checks if the path to the default camera position (cameraReturnPoint.transform.position) is blocked
            headPosition,
            direction,
            out hit,
            distance,
            layerMask,
            QueryTriggerInteraction.Ignore);

        Debug.DrawLine(
            headPosition,
            targetPosition,
            isCameraOccluded ? Color.red : Color.green);

        if (isCameraOccluded)
        {
            MoveCameraAroundOcclusion(hit.point);
        }
        else
        {
            ReturnCameraToPoint();
        }
    }

    void MoveCameraAroundOcclusion(Vector3 occlusionPoint)
    {
        cameraTransform.position = occlusionPoint;
    }

    void ReturnCameraToPoint()
    {
        cameraTransform.position = cameraReturnPoint.transform.position;
    }
}
