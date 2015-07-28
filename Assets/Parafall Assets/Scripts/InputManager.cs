using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	private string inputString;

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

	public ParaPacket findAndGrab(){
		foreach(ParaPacket paraPacket in parafallObjectPool.listOfPackets){
			List<GameObject> pooledGO = ParafallObjectPool.Instance.getObjectsOfType(paraPacket.paraName);
			foreach(GameObject tempGO in pooledGO){
				if(tempGO.activeSelf == true){
					GUIText childGuiTextObj = tempGO.transform.GetChild (0).guiText;
					//Debug.Log (childGuiTextObj.text);
					//Debug.Log ("input string : " + inputString);
					if(inputString.Equals(childGuiTextObj.text)){
						parafallObjectPool.putObjectBackToPool(tempGO);
						resetInputString ();
						return paraPacket;
					}
				}
			}
		}
		resetInputString ();
		return null;
	}

	public void resetInputString(){
		inputString = "";
	}
}
