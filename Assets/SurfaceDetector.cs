using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
	PlayerStats playerStats;
	[SerializeField] string surfaceTag;

	void Start()
	{
		playerStats = transform.root.GetComponent<PlayerStats>();
	}
	
    void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == surfaceTag)
		{
			playerStats.ReceivedContact(surfaceTag, collider);
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == surfaceTag)
		{
			playerStats.ExitContact(surfaceTag, collider);
		}
	}
}
