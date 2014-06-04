using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	private int noOfLives;

	private int playerScore;

	private int scoreRequiredToClearCurrentLevel;

	private int playerCurrentLevel;

	private float playerHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	public void setPlayerScore(int score){
		playerScore = score;
	}

	public void setPlayerHealth(int health){
		playerHealth = health;
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
}