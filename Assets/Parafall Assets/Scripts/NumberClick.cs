using UnityEngine;
using System.Collections;

public class NumberClick : MonoBehaviour {

	public GUIText testGuiText = null;

	//private ParafallObjectPool parafallObjectPool;

	private InputManager inputManager;

	// Use this for initialization
	void Start () {
		//parafallObjectPool = ParafallObjectPool.Instance;
		//inputManager = GameObject.Find ("GameManager").GetComponent<InputManager>();
		inputManager = InputManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount == 1){
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

		string buttonGuiText = this.guiText.text;
		if(!buttonGuiText.Equals ("GRAB")){
			if(testGuiText.text.Equals("FOUND") || testGuiText.text.Equals ("NOT FOUND"))
				testGuiText.text = buttonGuiText;
			else
				testGuiText.text = testGuiText.text + buttonGuiText;
			string inputStr = inputManager.appendInputString(buttonGuiText);
			Debug.Log ("input str : " + inputStr);
		}

		this.guiText.color = Color.red;
		if(guiText.text == "GRAB")
		{
			Debug.Log ("Grab button clicked.");
//			bool isObjFound = inputManager.findAndGrab();
//			if(isObjFound)
//				testGuiText.text = "FOUND";
//			else
//				testGuiText.text = "NOT FOUND";
		}

	}

	void OnMouseUp() {
		Debug.Log ("Mouse up on " + this.name);
		//testGuiText.text = "Mouse up on " + this.name;
		this.guiText.color = Color.white;
	}


}
