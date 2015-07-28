using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameData : MonoBehaviour {

	private int noOfLives;

	private int playerScore;

	private int coinsCount;

	private int scoreRequiredToClearCurrentLevel;

	private int playerCurrentLevel;

	private float playerHealth;

	private static GameData instance;

	private Dictionary<string, int> dictOfPowerUps = new Dictionary<string, int>();

	public delegate void PlayerScoreChangeAction (int score);
	public static event PlayerScoreChangeAction playerScoreChangeEvent;

	public delegate void PlayerHealthChangeAction (float health);
	public static event PlayerHealthChangeAction playerHealthChangeEvent;

	public delegate void PlayerPowerUpChangeAction (string type, int value);
	public static event PlayerPowerUpChangeAction playerPowerUpChangeEvent;

	public delegate void CoinsCountChangeAction (int count);
	public static event CoinsCountChangeAction coinsCountChangeEvent;
	
	public static GameData Instance {
		get{
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<GameData>();
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
		setPlayerHealth (10f);
	}
	
	public void setPlayerScore(int score){
		playerScore = score;
		playerScoreChangeEvent (playerScore);
	}

	public void setCoinsCount(int count){
		coinsCount = count;
		coinsCountChangeEvent (coinsCount);
	}

	public void setPlayerHealth(float health){
		if(health >= 0f){
			playerHealth = health;
			playerHealthChangeEvent (playerHealth);
		}
	}

	public void setNoOfLives(int lives){
		noOfLives = lives;
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

	public int getScoreRequiredToClearCurrentLevel(){
		return this.scoreRequiredToClearCurrentLevel;
	}

	public void setPowerUps(string type, int noOfPowerUps){
		//Debug.Log ("Setting power up : " + type + " : " + noOfPowerUps);
		if(noOfPowerUps >= 0){
			if (dictOfPowerUps.ContainsKey (type)) {
				dictOfPowerUps[type] = noOfPowerUps;
			} else {
				dictOfPowerUps.Add(type, noOfPowerUps);
			}
			playerPowerUpChangeEvent (type, noOfPowerUps);
		}

	}

	public int getPowerUpCount(string type){
		if (dictOfPowerUps.ContainsKey (type)) {
			return dictOfPowerUps[type];
		}

		return 0;
	}
}