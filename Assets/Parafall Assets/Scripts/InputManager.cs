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

	private string ruleInAction = "None";

	void OnEnable(){
		//ParafallEventManager.inputRuleChangeEvent += setRuleInAction;
	}

	void OnDisable(){
		//ParafallEventManager.inputRuleChangeEvent -= setRuleInAction;
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
		ParaPacket packetToReturn = null;
		bool packetFound = false;
		foreach(ParaPacket paraPacket in parafallObjectPool.listOfPackets){
			List<GameObject> pooledGO = ParafallObjectPool.Instance.getObjectsOfType(paraPacket.paraName);
			foreach(GameObject tempGO in pooledGO){
				if(tempGO.activeSelf == true && isPacketVisibleInCameraViewport(tempGO)){
					GUIText childGuiTextObj = tempGO.transform.GetChild (0).guiText;
					//Debug.Log (childGuiTextObj.text);
					//Debug.Log ("input string : " + inputString);
					switch(ruleInAction){
						case "None" : 
							if(inputString.Equals(childGuiTextObj.text)){
								parafallObjectPool.putObjectBackToPool(tempGO);
								packetToReturn = paraPacket;
								packetFound = true;
							}
							break;
						case "Reverse" :
							if(inputString.Equals (reverse (childGuiTextObj.text))){
								parafallObjectPool.putObjectBackToPool(tempGO);
								packetToReturn = paraPacket;
								packetFound = true;
							}
							break;
						case "AddDigits" :
							if(inputString.Equals (addDigits(childGuiTextObj.text))){
								parafallObjectPool.putObjectBackToPool(tempGO);
								packetToReturn = paraPacket;
								packetFound = true;
							}
							break;
						case "PlusOne" :
							if(inputString.Equals (plusOne(childGuiTextObj.text))){
								parafallObjectPool.putObjectBackToPool(tempGO);
								packetToReturn = paraPacket;
								packetFound = true;
							}
							break;
						case "MinusOne" :
							if(inputString.Equals (minusOne(childGuiTextObj.text))){
								parafallObjectPool.putObjectBackToPool(tempGO);
								packetToReturn = paraPacket;
								packetFound = true;
							}
							break;
					}
					if(packetFound)
						break;
				}
			}
		}
		resetInputString ();
		return packetToReturn;
	}

	public void resetInputString(){
		inputString = "";
	}

	public void setRuleInAction(string rule){
		ruleInAction = rule;
	}

	private string reverse(string str){
		string replacedStr = "";
		for (int i=str.Length-1; i>=0; i--) {
			replacedStr += str[i];		
		}

		return replacedStr;
	}

	private string addDigits(string str){
		int numberToReturn = 0;
		for (int i=0; i<str.Length; i++) {
			numberToReturn += int.Parse (str[i] + "");		
		}

		return numberToReturn.ToString();
	}

	private string plusOne(string str){
		int numberToReturn = 0;
		numberToReturn = int.Parse (str) + 1;

		return numberToReturn.ToString ();
	}

	private string minusOne(string str){
		int numberToReturn = 0;
		numberToReturn = int.Parse (str) - 1;
		
		return numberToReturn.ToString ();
	}

	private bool isPacketVisibleInCameraViewport(GameObject packetGO){
		Vector3 viewPos = Camera.main.WorldToViewportPoint (packetGO.transform.position);
		if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
				return true;
		else
				return false;
	}
}
