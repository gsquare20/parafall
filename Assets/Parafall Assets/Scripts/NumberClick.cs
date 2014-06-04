using UnityEngine;
using System.Collections;

public class NumberClick : MonoBehaviour {

	public GUIText testGuiText = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount == 0){
			Vector2 touchLocation = Input.GetTouch (0).position;
			TouchPhase touchPhase = Input.GetTouch (0).phase;
			if(touchPhase.Equals(TouchPhase.Began)){
				Ray r = Camera.main.ScreenPointToRay(touchLocation);
				RaycastHit hitInfo;
				if(Physics.Raycast(r, out hitInfo)){
					hitInfo.collider.gameObject.SendMessage("OnMouseDown");
				}
			}
		}
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
