using UnityEngine;

public class Crosshair : MonoBehaviour
{
	// GameObject playerObject;
	// PlayerStats playerStats;
    Animator anim;
    LayerMask designatedPlatformLayerMask;
    [SerializeField] RectTransform crosshairRectTransform;
    Vector3 screenCenter;
    float dogshitAimRadius = 1f;
    float platformInteractionDistance = 35f;
    float smoothSpeed = 30f;

    void Start()
    {
		// playerObject = GameObject.FindWithTag("Player");
		// playerStats = playerObject.GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        designatedPlatformLayerMask = LayerMask.GetMask("DesignatedPlatform");
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    void Update()
    {
        CheckForDesignatedPlatform();
    }

    void CheckForDesignatedPlatform()
    {
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        anim.SetBool("HoveringItem", false);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        Vector3 targetPos = screenCenter;

        // Raycast for picking up objects
        if (Physics.SphereCast(ray, dogshitAimRadius, out hit, platformInteractionDistance, designatedPlatformLayerMask))
        {
            targetPos = Camera.main.WorldToScreenPoint(hit.collider.transform.position);
            anim.SetBool("HoveringItem", true);
        }

        crosshairRectTransform.position = Vector3.Lerp(
            crosshairRectTransform.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}
