using UnityEngine;

public class Crosshair : MonoBehaviour
{
	GameObject playerObject;
	PlayerStats playerStats;
    Animator anim;
    LayerMask telepulsionPlatformLayerMask;
    [SerializeField] RectTransform crosshairRectTransform;
    Vector3 targetPos;
    Vector3 screenCenter;
    float dogshitAimRadius = 1f;
    float platformInteractionDistance = 35f;
    float smoothSpeed = 30f;

    void Start()
    {
		playerObject = GameObject.FindWithTag("Player");
		playerStats = playerObject.GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        telepulsionPlatformLayerMask = LayerMask.GetMask("TelepulsionPlatform");
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    void Update()
    {
        RaycastForTelepulsionPlatform();
        PositionCrosshair();
    }

    void RaycastForTelepulsionPlatform()
    {
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        targetPos = screenCenter;
        playerStats.isHoveringTelepulsionWall = false;

        // Raycast for picking up objects
        if (Physics.SphereCast(ray, dogshitAimRadius, out hit, platformInteractionDistance, telepulsionPlatformLayerMask))
        {
            targetPos = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            playerStats.isHoveringTelepulsionWall = true;
            playerStats.hoveredTelepulsionWall = hit.transform.gameObject;
        }
    }

    void PositionCrosshair()
    {
        anim.SetBool("HoveringItem", playerStats.isHoveringTelepulsionWall);

        crosshairRectTransform.position = Vector3.Lerp(
            crosshairRectTransform.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
