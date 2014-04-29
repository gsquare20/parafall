using UnityEngine;
using System.Collections;

public class MoveTextWithParachute : MonoBehaviour {

	public Transform target;
	Camera cam;
	public Vector3 offset = Vector3.up;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = cam.WorldToViewportPoint(target.position + offset);
	}
}
		                                                