using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour {
	public Transform[] lights;
	public ParticleSystem s;
	public Transform[] positions;
	private Vector3[] pos;
	// Use this for initialization
	void Start () {
		StartCoroutine(wait ());
		s.Play ();
		count = 0;
		pos = new Vector3[4];
		pos [0] = s.transform.position;
		for (int i=0; i<positions.Length; i++) {
			pos[i+1] = positions[i].position;
		}
	}

	int count;

	// Update is called once per frame

	void Update () {
	}

	IEnumerator wait() {
		if (count == 6) {
			count = 0;
			s.transform.position = pos[Random.Range(0,3)];
			s.Play();
		}
		turnOn ();
		yield return new WaitForSeconds(.245f);
		turnOff ();
		yield return new WaitForSeconds(.245f);
		count++;
		StartCoroutine(wait ());
	}

	void turnOn(){
		Debug.Log ("on");
		for (int i=0; i<lights.Length; i++)
			lights [i].gameObject.SetActive (true);
	}
	void turnOff(){
		Debug.Log ("off");
		for (int i=0; i<lights.Length; i++)
			lights [i].gameObject.SetActive (false);
	}
}
