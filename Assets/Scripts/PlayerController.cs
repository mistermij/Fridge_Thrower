using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
	
	// Handling
	public float speed = 5;
	
	
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
		
		Vector3 motion = input;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)?.7f:1;
		motion *= speed;
		motion += Vector3.up * -8;
		
		controller.Move(motion * Time.deltaTime);
		
		if (Input.GetKey (KeyCode.Q) || Input.GetKeyDown (KeyCode.Joystick1Button0)) {
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
		Debug.Log ("reset");
		dash_end = Time.time;
		dashing = false;
	}
	
	void dash(Vector3 direction) 
	{	
		//cooldown check
		if (dash_cooldown + dash_end > Time.time)
			return;
		Debug.Log ("vroom");
		//start dash if it hasn't already started
		if (!dashing) {
			dashing = !dashing;
			dash_start = Time.time;
			Debug.Log ("start");
		} else 
			if (dash_start + dash_duration >= Time.time) {
				Debug.Log("Moving");
				controller.Move (direction * 10 * dash_curve.Evaluate (Time.time - dash_start) * Time.deltaTime);
			}
		else {
			dashing = !dashing;
			dash_end = Time.time;
			Debug.Log ("end");
		}
	}
}