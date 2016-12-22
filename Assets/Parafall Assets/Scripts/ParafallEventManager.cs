using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class ParaEventObject : System.Object{
	
	public int gameScore;
	public bool rule;
	public enum Rules{ Reverse, AddDigits, PlusOne, MinusOne, None}
	public Rules ruleValue;
	public bool changeParapacketsSpeed;
	public List<ParaSpeedChangeObject> paraSpeedChangeObjList;
	public bool swapNumbersOnScreen;


}

[System.Serializable]
public class ParaSpeedChangeObject : System.Object{
	public float speed;
	public int parafallSpawnerNo;
}

[System.Serializable]
public class RuleObject : System.Object{
	public string ruleName;
	public string ruleDesc;
}

public class ParafallEventManager : MonoBehaviour {

	private Dictionary<int, ParaEventObject> paraEventDict = new Dictionary<int, ParaEventObject>();

	private Dictionary<string, RuleObject> paraRuleDict = new Dictionary<string, RuleObject>();

	public List<ParaEventObject> paraEventObjList = new List<ParaEventObject>();

	public List<RuleObject> paraRuleObjList = new List<RuleObject>();

	public delegate void InputRuleChangeAction (string rule);
	public static event InputRuleChangeAction inputRuleChangeEvent;
	
	private GameData gameData;
	private InputManager inputManager;
	public GameObject modalPanel;
	public GameObject rulePopUp;
	public Text ruleValue;
	public Text ruleDesc;
	private UIButtonClick uiButtonClick;



	void OnEnable() {
		//Debug.Log ("enabling playerScoreChangeEvent in parafall event manager.");
		GameData.playerScoreChangeEvent += checkEventManager;
	}

	void OnDisable() {
		//Debug.Log ("disabling playerScoreChangeEvent in parafall event manager.");
		GameData.playerScoreChangeEvent -= checkEventManager;
	}
	
	// Use this for initialization
	void Start () {
		uiButtonClick = UIButtonClick.Instance;
		gameData = GameData.Instance;
		inputManager = InputManager.Instance;
		foreach (ParaEventObject paraEventObj in paraEventObjList) {
			paraEventDict.Add (paraEventObj.gameScore, paraEventObj);		
		}

		foreach (RuleObject paraRuleObj in paraRuleObjList) {
			paraRuleDict.Add (paraRuleObj.ruleName, paraRuleObj);		
		}
		//Debug.Log (paraEventDict.Count);
		//Debug.Log (paraEventDict.Keys);
	}



	void checkEventManager (int playerScore) {
		//Debug.Log ("player Score is : " + playerScore.ToString());
		if (paraEventDict.ContainsKey (playerScore)) {
			Debug.Log ("Do boom boom!");	
			ParaEventObject paraEventObj = paraEventDict[playerScore];

			if(paraEventObj.rule){
				initiateRuleSettings(paraEventObj.ruleValue);
			}else{
				initiateRuleSettings(ParaEventObject.Rules.None);
			}

			if(paraEventObj.changeParapacketsSpeed){
				initiateParapacketsSpeedChange(paraEventObj.paraSpeedChangeObjList);
			}

			if(paraEventObj.swapNumbersOnScreen){
				initiateNumbersSwapOnScreen();
			}
		}
	}

	private void initiateRuleSettings(ParaEventObject.Rules ruleValue){

		Time.timeScale = 0;
		modalPanel.SetActive (true);
		rulePopUp.SetActive (true);
		AdManager.Instance.showInterstitialAd ();
		string ruleValueStr = ruleValue.ToString ();
		this.ruleValue.text = ruleValueStr;
		this.ruleDesc.text = getRuleDetails (ruleValueStr).ruleDesc;
		Debug.Log (inputRuleChangeEvent==null);

		switch (ruleValue){
			case ParaEventObject.Rules.None :
			inputManager.setRuleInAction("None");
				break;
			case ParaEventObject.Rules.Reverse : 
			inputManager.setRuleInAction("Reverse");
				break;
			case ParaEventObject.Rules.AddDigits :
			inputManager.setRuleInAction("AddDigits");
				break;
			case ParaEventObject.Rules.PlusOne :
			inputManager.setRuleInAction("PlusOne");
				break;
			case ParaEventObject.Rules.MinusOne :
			inputManager.setRuleInAction("MinusOne");
				break;
		}
	}

	private void initiateParapacketsSpeedChange(List<ParaSpeedChangeObject> paraSpeedChangeObjList){
		foreach (ParaSpeedChangeObject paraSpeedChangeObj in paraSpeedChangeObjList) {
			int spawnerNo = paraSpeedChangeObj.parafallSpawnerNo;
			float newSpeed = paraSpeedChangeObj.speed;
			GameObject parachuteSpawners = GameObject.FindGameObjectWithTag("Parachute Spawners");
			GameObject requiredSpawner = parachuteSpawners.transform.GetChild(spawnerNo - 1).gameObject;
			ParachuteSpawner parachuteSpawner = requiredSpawner.GetComponent<ParachuteSpawner>();
			parachuteSpawner.fallSpeed = newSpeed;
		}
	}

	private void initiateNumbersSwapOnScreen(){
		GameObject gameButtons = GameObject.FindWithTag ("Game Buttons");
		GameObject gameButtonsHidden = GameObject.FindWithTag ("Game Buttons Hidden");

		//Debug.Log (gameButtons.name);
		//Debug.Log (gameButtonsHidden.name);

		Component[] gameButtonsRectTransforms = gameButtons.transform.GetComponentsInChildren<Button> (true);
		//Debug.Log ("game buttons count : " + gameButtonsRectTransforms.Length);
		foreach (Button rectTransform in gameButtonsRectTransforms) {
			//Debug.Log ("Is active self : " + rectTransform.gameObject.activeSelf);
			rectTransform.gameObject.SetActive (false);		
		}

		Component[] gameButtonsHiddenRectTransforms = gameButtonsHidden.transform.GetComponentsInChildren<Button> (true);
		//Debug.Log ("game buttons hidden count : " + gameButtonsHiddenRectTransforms.Length);
		foreach (Button rectTransform in gameButtonsHiddenRectTransforms) {
			//Debug.Log ("Is active self : " + rectTransform.gameObject.activeSelf);
			rectTransform.gameObject.SetActive (true);		
		}

		gameButtonsHidden.transform.SetAsLastSibling ();

		gameButtons.tag = "Game Buttons Hidden";
		gameButtonsHidden.tag = "Game Buttons";
	}

	public static int IntParseFast(string value){
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}

	public void disableRulePopUp(){
		modalPanel.SetActive (false);
		rulePopUp.SetActive (false);
		Time.timeScale = 1;
	}

	public RuleObject getRuleDetails(string ruleName){
		if(null != paraRuleDict && paraRuleDict.ContainsKey(ruleName))
			return paraRuleDict[ruleName];

		return null;
	}
}