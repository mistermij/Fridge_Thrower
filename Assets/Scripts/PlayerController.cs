using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
	
	// Handling
	public float speed = 5;

	private bool facingRight = true;

	// Components
	private CharacterController controller;
	private Animator animator;
	
	void Start () {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator> ();
	}
	
	void Update () {
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		
		animator.SetBool ("Running", input.magnitude > 0);

		//Do I need to Flip ? 
		if (input.x < 0 && facingRight) {
			Flip ();
		} else if (input.x > 0 && !facingRight) {
			Flip ();
		}
		
		Vector3 motion = input;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)?.7f:1;
		motion *= speed;
		motion += Vector3.up * -8;
		
		controller.Move(motion * Time.deltaTime);
		
		if (Input.GetButton("Dash")) {
			dash (input);
		}
		else 
			if (dashing)
				dashReset ();
	}
	
	void Direction(){
		
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) {
		if (hit.transform.tag == "Pickable") 
		{
			hit.transform.parent = this.transform;
			hit.transform.localPosition = new Vector3 (0.015f, 0.705f, 0.541f);
			hit.transform.rotation = Quaternion.Euler (7.531f, 356.223f, 29.632f);
			animator.SetBool ("Holding", true);
		}
	}
	
	
	//OLH H OYSIA TOU GAME EINAI EDW
	private bool dashing = false;	//dash flag
	private float dash_start;		//time dash started
	private float dash_end;			//time dash stopped
	
	[Header("Dash")]
	public float dash_duration;		
	public float dash_cooldown;		
	public AnimationCurve dash_curve;
	
	void dashReset() {
		dash_end = Time.time;
		dashing = false;
	}
	
	void dash(Vector3 direction) 
	{	
		//cooldown check
		if (dash_cooldown + dash_end > Time.time)
			return;
		//start dash if it hasn't already started
		if (!dashing) {
			dashing = !dashing;
			dash_start = Time.time;
			//Debug.Log ("start");
		} else 
			if (dash_start + dash_duration >= Time.time) {
				controller.Move (direction * (2*speed) * dash_curve.Evaluate (Time.time - dash_start) * Time.deltaTime);
			}
		else {
			dashing = !dashing;
			dash_end = Time.time;
		}
	}

	//Flips the Sprite when changing direction
	void Flip()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}