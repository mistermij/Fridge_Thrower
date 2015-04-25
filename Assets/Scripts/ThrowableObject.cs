using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour {

	public 	Transform Handle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void pickUp(Transform handlingPosition){
		transform.parent = handlingPosition.transform;
		transform.rotation = handlingPosition.transform.rotation;

		transform.localPosition = new Vector3(0.092f , 0.576f , 0.299f);
	}
}
