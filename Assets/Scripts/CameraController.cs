using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//Handling
	public float cameraDistance = 3.6f;

	//System
	private Vector3 cameraTarget;
	
	private Transform target;
	
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update () {
		if (target) {
			cameraTarget = new Vector3 (target.position.x, transform.position.y, target.position.z-cameraDistance);
			transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.deltaTime * 8);
		
		} 
	}
}
