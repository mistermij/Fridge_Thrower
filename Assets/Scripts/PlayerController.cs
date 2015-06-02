using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
	
	// Handling
	public float speed = 5;
	public ThrowableObject equipedObject;
	public Transform handlingPosition;
	public Transform fwdPosition;

	public float reachDistance;

	private bool facingRight = true;
	private bool holding = false;

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
		
		//Dash
		if (Input.GetButton("Dash") && !holding) {
			dash (input);
		}
		else 
			if (dashing)
				dashReset ();

		//PickUpObject
		//TODO: dash availability while holding based on object's weight
		if (Input.GetButtonDown("Action") && !holding) {
			TryGrab();
		}
		if (Input.GetButtonDown ("Throw") && holding) {
			Throw (input);
		}
	}

	void TryGrab(){
		
		RaycastHit hit;
		Ray ray;

		//RayCasting settings
		int totalRaysY = 5;
		int totalRaysX = 3;
		float spreadY = 1;
		float spreadX = 1;

		for(int i=0 ; i<totalRaysX; i++){
			for (int j=0; j<totalRaysY; j++) {

				Vector3 rayPos = new Vector3(fwdPosition.position.x , 
				                             fwdPosition.position.y - j*(spreadY/totalRaysY) , 
				                             fwdPosition.position.z + spreadX/2 - (i+1)*(spreadX/totalRaysX));

				ray =  new Ray(rayPos , fwdPosition.transform.TransformDirection (Vector3.forward) );

				Debug.DrawRay (ray.origin, ray.direction, Color.green);
			
				if (Physics.Raycast (ray, out hit, reachDistance)) {
					if (hit.transform.tag == "Pickable") {

						//Debug.Log ("Found a pickable Object");
						equipedObject = hit.collider.GetComponent (typeof(ThrowableObject)) as ThrowableObject;
						equipedObject.pickUp (handlingPosition);

						holding = true;
						animator.SetBool ("Holding", true);
					}
				}
			}
		}
	}


	private void HoldItem() 
	{
		holding = true;
		animator.SetBool ("Holding", true);
	}

	private void StopHolding() {
		holding = false;
		animator.SetBool("Holding",false);
		//equipedObject = null;
	}
	
	//OLH H OYSIA TOU GAME EINAI EDW
	//TODO: dash should have a minimum duration.
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

		//Fliping forward position (den kserw gt douleuei alla douleuei)
		//TODO:Kill the one who wrote the following statement. !important
		//fwdPosition.transform.localPosition = new Vector3(fwdPosition.transform.localPosition.x,fwdPosition.transform.localPosition.y,fwdPosition.transform.localPosition.z);
		fwdPosition.transform.eulerAngles += 180f * Vector3.up;
	}

	public Vector3 throw_power = new Vector3(15,3,15);

	//Throw function
	void Throw(Vector3 input){
		//determine direction
		float x = input.x, z=input.z;
		if (z*x != 0) 
		{
			z *= .707f;
			x *= .707f;
		}

		//player's side
		StopHolding ();
		//item's side
		Physics.IgnoreCollision (equipedObject.transform.GetComponent<Collider> (), transform.GetComponent<Collider> ());
		equipedObject.transform.parent = null;
		equipedObject.SendMessage ("Throw",new Vector3(x*15,3,z*15));
		equipedObject = null;
	}
}