using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public 	Transform Handle;
	private Vector3 initial_scale;
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		initial_scale = transform.localScale;
		rigidbody = transform.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (rigidbody.velocity.magnitude < 0.5)
			rigidbody.isKinematic = true;
	}

	public void pickUp(Transform handlingPosition){
		transform.parent = handlingPosition.transform;
		transform.rotation = handlingPosition.transform.rotation;

		transform.localPosition = new Vector3(0.092f , 0.576f , 0.299f);
	}

	public void Throw(Vector3 direction){
		//gia to random scaling p kanei otn t petaei
		transform.localScale = initial_scale;
		rigidbody.isKinematic = false;
		rigidbody.velocity = direction;
	}
}
