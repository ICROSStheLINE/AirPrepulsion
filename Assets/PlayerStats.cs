using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	Animator anim;
	
    [HideInInspector] public bool isTouchingFloor = true;
	
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Floor")
		{
			isTouchingFloor = true;
			anim.SetBool("FallingIdle", false);
			Debug.Log("Entered Floor");
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Floor")
		{
			isTouchingFloor = false;
			anim.SetBool("FallingIdle", true);
			Debug.Log("Exited Floor");
		}
	}
}
