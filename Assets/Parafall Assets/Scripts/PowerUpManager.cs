using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PowerUpItem : System.Object{

	public string powerUpName;
	public string powerUpText;
	//public string powerUpValue;
	public string powerUpDesc;
}

public class PowerUpManager : MonoBehaviour {

	private static PowerUpManager instance;

	public static PowerUpManager Instance {
		get {
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<PowerUpManager>();
			}
			
			return instance;
		}
	}

	private Dictionary<int, IPlayerPowerUp> powerUpsDict = new Dictionary<int, IPlayerPowerUp>();

	public List<PowerUpItem> powerUpList = new List<PowerUpItem> ();

	public GameObject powerUpGOToInstantiate;

	public GameObject powerUpMenu;

	private GameObject powerUpMenuPanel;

	private GameData gameData;

	public GameObject powerUpSlider;

	public delegate void PowerUpInUseAction (string powerUpName);
	public static event PowerUpInUseAction powerUpInUseEvent;

	public delegate void PowerUpNotInUseAction ();
	public static event PowerUpNotInUseAction powerUpNotInUseEvent;

	public enum PowerUpEnum {
		DoubleTheCoinPowerUp,
		DoubleTheScorePowerUp,
		SlowDownFallPowerUp,
		AutoGrabPowerUp
	}

	void Awake(){
		if(null != instance)
			DestroyImmediate(gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
		powerUpMenuPanel = powerUpMenu.transform.FindChild ("Power Ups Panel").gameObject;
		powerUpsDict.Add ((int)PowerUpEnum.DoubleTheCoinPowerUp, new DoubleTheCoinPowerUp (this));
		powerUpsDict.Add ((int)PowerUpEnum.DoubleTheScorePowerUp, new DoubleTheScorePowerUp (this));
		powerUpsDict.Add ((int)PowerUpEnum.SlowDownFallPowerUp, new SlowDownFallPowerUp (this));
		powerUpsDict.Add ((int)PowerUpEnum.AutoGrabPowerUp, new AutoGrabPowerUp (this));

		generatePowerUpMenu ();

		gameData = GameData.Instance;


	}

	void OnEnable(){
		GameData.playerPowerUpChangeEvent += updatePowerUpValueInMenu;
	}

	void OnDisable(){
		GameData.playerPowerUpChangeEvent -= updatePowerUpValueInMenu;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void generatePowerUpMenu (){
		foreach (PowerUpItem powerUpItem in powerUpList) {
			GameObject powerUpGO = (GameObject) Instantiate (powerUpGOToInstantiate);
			if(null != powerUpGO){
				powerUpGO.transform.SetParent (powerUpMenuPanel.transform);
				PowerUpItemScript powerUpItemScript = powerUpGO.GetComponent<PowerUpItemScript>();
				powerUpItemScript.powerUpItemButtonText.text = powerUpItem.powerUpText;
				//powerUpItemScript.powerUpItemValue.text = powerUpItem.powerUpValue;
				powerUpItemScript.powerUpItemName = powerUpItem.powerUpName;
				Button powerUpButton = powerUpGO.transform.FindChild("PowerUpButton").GetComponent<Button>();
				switch (powerUpItemScript.powerUpItemName){
					case "graballpowerup":
						powerUpButton.onClick.AddListener(() => useAutoGrabPowerUp());
						break;

					case "slowdownfallpowerup":
						powerUpButton.onClick.AddListener(() => useSlowDownFallPowerUp());
						break;

					case "doublethescorepowerup":
						powerUpButton.onClick.AddListener(() => useDoubleTheScorePowerUp());
						break;

					case "doublethecoinpowerup":
						powerUpButton.onClick.AddListener(() => useDoubleTheCoinPowerUp());
						break;
					
				}
			}
		}

		Debug.Log ("Power Up Menu instantiated.");
	}

	public void openPowerUpMenu(){
		refreshPowerUpValuesOnMenuOpen ();
		powerUpMenu.SetActive (true);
		Time.timeScale = 0;
	}

	public void closePowerUpMenu(){
		powerUpMenu.SetActive (false);
		Time.timeScale = 1;
	}

	public IPlayerPowerUp getPowerUp(PowerUpEnum powerUpEnum){
		int powerUp = (int)powerUpEnum;
		return powerUpsDict[powerUp];
	}

	public void useDoubleTheCoinPowerUp(){
		int powerUpCount = decrementPowerUpCount ("doublethecoinpowerup");
		closePowerUpMenu ();

		if(powerUpCount >= 0){
			getPowerUp (PowerUpEnum.DoubleTheCoinPowerUp).executePowerUpRelatedTasks (powerUpSlider);
			activatePowerUpSliderToValue (30f);
			powerUpInUseEvent("doublethecoinpowerup");
			InvokeRepeating ("waitForASecond", 1f, 1f);
		}

	}

	public void useDoubleTheScorePowerUp(){
		int powerUpCount = decrementPowerUpCount ("doublethescorepowerup");
		closePowerUpMenu ();

		if(powerUpCount >= 0){
			getPowerUp (PowerUpEnum.DoubleTheScorePowerUp).executePowerUpRelatedTasks (powerUpSlider);
			activatePowerUpSliderToValue (10f);
			powerUpInUseEvent("doublethescorepowerup");
			InvokeRepeating ("waitForASecond", 1f, 1f);
		}

	}

	public void useSlowDownFallPowerUp(){
		int powerUpCount = decrementPowerUpCount ("slowdownfallpowerup");
		closePowerUpMenu ();

		if(powerUpCount >= 0){
			getPowerUp (PowerUpEnum.SlowDownFallPowerUp).executePowerUpRelatedTasks (powerUpSlider);
			activatePowerUpSliderToValue (10f);
			powerUpInUseEvent("slowdownfallpowerup");
			InvokeRepeating ("waitForASecond", 1f, 1f);
		}

	}

	public void useAutoGrabPowerUp(){
		int powerUpCount = decrementPowerUpCount ("graballpowerup");
		closePowerUpMenu ();

		if(powerUpCount >= 0){
			getPowerUp (PowerUpEnum.AutoGrabPowerUp).executePowerUpRelatedTasks (powerUpSlider);
			activatePowerUpSliderToValue (10f);
			powerUpInUseEvent("graballpowerup");
			InvokeRepeating ("waitForASecond", 1f, 1f);
		}

	}

	private int decrementPowerUpCount(string powerUpType){
		int powerUpCount = gameData.getPowerUpCount (powerUpType) - 1;
		gameData.setPowerUps(powerUpType, powerUpCount, true);
		return powerUpCount;
	}

	void updatePowerUpValueInMenu(string powerUpType, int powerUpValue){
		//Debug.Log ("Updating power up value in menu with : " + powerUpType + " and " + powerUpValue);
		foreach(PowerUpItemScript powerUpItemScript in powerUpMenuPanel.GetComponentsInChildren<PowerUpItemScript>(true)){
			//Debug.Log ("power up item name : " + powerUpItemScript.powerUpItemName);
			if(powerUpItemScript.powerUpItemName.Equals(powerUpType)){
				//Debug.Log ("power up script found.");
				powerUpItemScript.transform.FindChild("PowerUpValue").gameObject.GetComponent<Text>().text = powerUpValue.ToString();
				break;
			}
		}
	}

	void refreshPowerUpValuesOnMenuOpen(){
		Dictionary<string, int> tempDictOfPowerUps = gameData.getPowerUps ();
		Dictionary<string, int> tempPowerUpsDict = new Dictionary<string, int> (tempDictOfPowerUps);
		foreach (string key in tempPowerUpsDict.Keys) {
			gameData.setPowerUps (key, tempDictOfPowerUps[key], true);		
		}
		tempPowerUpsDict = null;
	}

	void waitForASecond(){
		Slider slider = powerUpSlider.GetComponent<Slider> ();
		if (slider.value > 0f)
			slider.value -= 1f;
		if (slider.value == 0f){
			powerUpSlider.SetActive (false);
			CancelInvoke ("waitForASecond");
			powerUpNotInUseEvent();
		}

	}


	private void activatePowerUpSliderToValue(float sliderValue){
		Slider slider = powerUpSlider.GetComponent<Slider> ();
		slider.maxValue = sliderValue;
		slider.value = sliderValue;
		if (powerUpSlider.activeSelf){
			CancelInvoke ("waitForASecond");
			powerUpNotInUseEvent();
		}
		powerUpSlider.SetActive (true);
	}
}
