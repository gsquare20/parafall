using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	private static string inputString;

	private string objectTypeToPreInstantiate = "parachute";

	private ParafallObjectPool parafallObjectPool;

	private static InputManager instance;

	public static InputManager Instance {
		get{
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<InputManager>();
			}

			return instance;
		}
	}

	void Awake(){
		if(null != instance)
			DestroyImmediate(gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
		inputString = "";
		parafallObjectPool = ParafallObjectPool.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string appendInputString(string strToAppend){
		string tempStr = inputString + strToAppend;
		inputString = tempStr;
		return inputString;
	}

	public bool findAndGrab(){
		List<GameObject> pooledGO = ParafallObjectPool.Instance.getObjectsOfType(objectTypeToPreInstantiate);
		foreach(GameObject tempGO in pooledGO){
			if(tempGO.activeSelf == true){
				GUIText childGuiTextObj = tempGO.transform.GetChild (0).guiText;
				//Debug.Log (childGuiTextObj.text);
				//Debug.Log ("input string : " + inputString);
				if(inputString.Equals(childGuiTextObj.text)){
					ParafallObjectPool.Instance.putObjectBackToPool(tempGO);
					resetInputString ();
					return true;
				}
			}
		}
		resetInputString ();
		return false;
	}

	public void resetInputString(){
		inputString = "";
	}
}
