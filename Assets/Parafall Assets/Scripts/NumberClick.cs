using UnityEngine;
using System.Collections;

public class NumberClick : MonoBehaviour {

	public GUIText testGuiText = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log ("Mouse down on " + this.name);
		testGuiText.text = "Mouse down on " + this.name;
		this.guiText.color = Color.red;
	}

	void OnMouseUp() {
		Debug.Log ("Mouse up on " + this.name);
		testGuiText.text = "Mouse up on " + this.name;
		this.guiText.color = Color.white;
	}
}
