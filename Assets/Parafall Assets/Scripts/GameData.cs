using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class GameDataSerialized{
	public int playerLastHighestScore;
	public int playerTotalCoinsCount;
	public int playerNoOfLives;
	public int playerHealthBars;
	public List<PlayerPowerUpSerialized> playerPowerUpList = new List<PlayerPowerUpSerialized>();
}

[System.Serializable]
public class PlayerPowerUpSerialized {
	public string powerUpType;
	public int powerUpValue;
}

public class GameData : MonoBehaviour {

	private int noOfLives;

	private int playerScore;

	private int playerHighestScore;

	private int coinsCount;

	private int playerTotalCoinsCount;

	private int tempTotalCoinsCount;

	private int scoreRequiredToClearCurrentLevel;

	private int playerCurrentLevel;

	private int playerHealthBars;

	private float playerHealth;

	private static GameData instance;

	private Dictionary<string, int> dictOfPowerUps = new Dictionary<string, int>();

	private PlayerController playerController;

	public delegate void PlayerScoreChangeAction (int score);
	public static event PlayerScoreChangeAction playerScoreChangeEvent;

	public delegate void PlayerHighestScoreChangeAction (int score);
	public static event PlayerHighestScoreChangeAction playerHighestScoreChangeEvent;

	public delegate void PlayerHealthChangeAction (float health);
	public static event PlayerHealthChangeAction playerHealthChangeEvent;

	public delegate void PlayerPowerUpChangeAction (string type, int value);
	public static event PlayerPowerUpChangeAction playerPowerUpChangeEvent;

	public delegate void CoinsCountChangeAction (int count);
	public static event CoinsCountChangeAction coinsCountChangeEvent;

	public delegate void TotalCoinsCountChangeAction (int count);
	public static event TotalCoinsCountChangeAction totalCoinsCountChangeEvent;

	public delegate void PlayerHealthBarsCountChangeAction (int count);
	public static event PlayerHealthBarsCountChangeAction playerHealthBarsCountChangeEvent;

	public delegate void PlayerNoOfLivesChangeAction (int count);
	public static event PlayerNoOfLivesChangeAction playerNoOfLivesChangeEvent;
	
	public static GameData Instance {
		get{
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<GameData>();
			}
			
			return instance;
		}
	}

	void OnEnable(){
		StateManager.endStateEvent += setTotalCoinsAndHighestScoreOnGameEnd;
		PlayerController.loadGameDataEvent += loadGameDataFromFile;
		PlayerController.liveAgainPowerUpUsedEvent += setPowerHealthToFull;
		//loadGameDataFromFile ();
	}

	void OnDisable(){
		StateManager.endStateEvent -= setTotalCoinsAndHighestScoreOnGameEnd;
		PlayerController.loadGameDataEvent -= loadGameDataFromFile;
		PlayerController.liveAgainPowerUpUsedEvent -= setPowerHealthToFull;
		persistGameDataToFile ();
	}

	void Awake(){

		instance = this;
	}

	// Use this for initialization
	void Start () {
		setPlayerHealth (10f);
		playerController = PlayerController.Instance;
	}
	
	public void setPlayerScore(int score){
		playerScore = score;
		if (playerScore > playerHighestScore)
			playerHighestScore = playerScore;
		playerScoreChangeEvent (playerScore);
	}

	public void setCoinsCount(int count){
		coinsCount = count;
		tempTotalCoinsCount = playerTotalCoinsCount + coinsCount;
		coinsCountChangeEvent (coinsCount);
	}

	public void setTotalCoinsCount(int count){
		this.playerTotalCoinsCount = count;
	}

	public void setPlayerHealthBars(int count){
		this.playerHealthBars = count;
		playerHealthBarsCountChangeEvent (count);
	}

	public void setPlayerHealth(float health){
		//Debug.Log ("Player Health : " + health);
		if(health >= 0f){
			playerHealth = health;
			playerHealthChangeEvent (playerHealth);
		}
	}

	public void setNoOfLives(int lives){
		noOfLives = lives;
		playerNoOfLivesChangeEvent (noOfLives);
	}

	public void setPlayerCurrentLevel(int currentLevel){
		playerCurrentLevel = currentLevel;
	}

	public void setScoreRequiredToClearCurrentLevel(int scoreRequired){
		scoreRequiredToClearCurrentLevel = scoreRequired;
	}

	public int getPlayerScore(){
		return this.playerScore;
	}

	public int getCoinsCount(){
		return this.coinsCount;
	}

	public float getPlayerHealth(){
		return this.playerHealth;
	}

	public int getNoOfLives(){
		return this.noOfLives;
	}

	public int getTotalCoinsCount() {
		return this.playerTotalCoinsCount;	
	}

	public int getScoreRequiredToClearCurrentLevel(){
		return this.scoreRequiredToClearCurrentLevel;
	}

	public int getPlayerHealthBarsCount(){
		return this.playerHealthBars;	
	}

	public void setPowerUps(string type, int noOfPowerUps, bool raiseEvent){
		//Debug.Log ("Setting power up : " + type + " : " + noOfPowerUps);
		if(noOfPowerUps >= 0){
			if (dictOfPowerUps.ContainsKey (type)) {
				dictOfPowerUps[type] = noOfPowerUps;
			} else {
				dictOfPowerUps.Add(type, noOfPowerUps);
			}
			if(raiseEvent)
				playerPowerUpChangeEvent (type, noOfPowerUps);
		}

	}

	public int getPowerUpCount(string type){
		if (dictOfPowerUps.ContainsKey (type)) {
			return dictOfPowerUps[type];
		}

		return 0;
	}

	public Dictionary<string, int> getPowerUps(){
		return dictOfPowerUps;
	}

	private void loadGameDataFromFile(){
		if (File.Exists (Application.persistentDataPath + "/pinfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open (Application.persistentDataPath + "/pinfo.dat", FileMode.Open);
			GameDataSerialized gameDataSerializedObj = (GameDataSerialized) bf.Deserialize(fileStream);
			fileStream.Close ();

			playerHighestScore = gameDataSerializedObj.playerLastHighestScore;
			noOfLives = gameDataSerializedObj.playerNoOfLives;
			playerHealthBars = gameDataSerializedObj.playerHealthBars;
			playerHealthBarsCountChangeEvent(playerHealthBars);
			playerTotalCoinsCount = gameDataSerializedObj.playerTotalCoinsCount;

			foreach(PlayerPowerUpSerialized playerPowerUpSerialized in gameDataSerializedObj.playerPowerUpList){
				setPowerUps (playerPowerUpSerialized.powerUpType, playerPowerUpSerialized.powerUpValue, false);
			}
		}
		Debug.Log ("Player Data successfully loaded.");
	}

	private void persistGameDataToFile() {

		if(tempTotalCoinsCount > playerTotalCoinsCount)
			playerTotalCoinsCount = tempTotalCoinsCount;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Open (Application.persistentDataPath + "/pinfo.dat", FileMode.OpenOrCreate);

		GameDataSerialized gameDataSerializedObj = new GameDataSerialized ();
		gameDataSerializedObj.playerLastHighestScore = playerHighestScore;
		gameDataSerializedObj.playerNoOfLives = noOfLives;
		gameDataSerializedObj.playerHealthBars = playerHealthBars;
		gameDataSerializedObj.playerTotalCoinsCount = playerTotalCoinsCount;

		foreach (string key in dictOfPowerUps.Keys) {
			PlayerPowerUpSerialized playerPowerUpSerializedObj = new PlayerPowerUpSerialized();
			playerPowerUpSerializedObj.powerUpType = key;
			playerPowerUpSerializedObj.powerUpValue = dictOfPowerUps[key];
			gameDataSerializedObj.playerPowerUpList.Add(playerPowerUpSerializedObj);		
		}

		bf.Serialize (fileStream, gameDataSerializedObj);
		fileStream.Close ();

		Debug.Log ("Player Data successfully persisted.");
	}

	void setTotalCoinsAndHighestScoreOnGameEnd(){
		if(tempTotalCoinsCount > playerTotalCoinsCount)
			playerTotalCoinsCount = tempTotalCoinsCount;

		playerHighestScoreChangeEvent (playerHighestScore);
		totalCoinsCountChangeEvent (playerTotalCoinsCount);
	}

	public void fillHealthBar(){
		if (playerHealthBars > 0) {
			setPlayerHealthBars(getPlayerHealthBarsCount() - 1);
			if(playerHealth <= 5f)
				setPlayerHealth (getPlayerHealth () + 5f);
			else
				setPlayerHealth(10f);
		}else{
			playerController.showDefaultPopUp("Not enough health bars to refill health!");
		}
	}

	private void setPowerHealthToFull(){
		setPlayerHealth (10f);
		setNoOfLives (getNoOfLives () - 1);
	}
}