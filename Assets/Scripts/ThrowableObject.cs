using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public 	Transform Handle;
	private Vector3 initial_scale;
	private Rigidbody rigidbody;
	private Vector3 originalScale;
	private Transform player;
	// Use this for initialization
	void Start () {
		initial_scale = transform.localScale;
		rigidbody = transform.GetComponent<Rigidbody> ();
		originalScale = transform.localScale;
		player = FindObjectOfType<PlayerController> ().transform; //TOO SLOW...T.T
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		//make rigidbody kinematic again when it stops moving
		if (rigidbody.velocity.magnitude == 0 && rigidbody.isKinematic == false) {
			rigidbody.isKinematic = true;	
			collideAgain ();
		}

	}

	public void pickUp(Transform handlingPosition){
		//make rigidbody kinematic before pick up 
		rigidbody.isKinematic = true;
		//fix items position
		transform.parent = handlingPosition.transform;
		transform.rotation = handlingPosition.transform.rotation;
		transform.localPosition = new Vector3(0.092f , 0.576f , 0.299f);
		//make sure it maintains its original scale while held
		transform.localScale = originalScale;
	}

	public void Throw(Vector3 direction){
		//gia to random scaling p kanei otn t petaei
		transform.localScale = initial_scale;
		rigidbody.isKinematic = false;
		rigidbody.velocity = direction;
	}

	//re-enable collision between player and object.
	public void collideAgain() {
		Physics.IgnoreCollision (GetComponent<Collider> (), player.GetComponent<Collider>(), false);
	}
}
