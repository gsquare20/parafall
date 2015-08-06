using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Text scoreText;

	public Text coinsText;

	public Text endMenuScoreText;

	public Text endMenuCoinsText;

	public Text endMenuHighestScoreText;
	
	public Text endMenuTotalCoinsText;
	
	public Slider healthSlider;

	void OnEnable() {
		GameData.playerScoreChangeEvent += setScoreText;
		GameData.playerHealthChangeEvent += setHealthSlider;
		GameData.coinsCountChangeEvent += setCoinsText;
		GameData.totalCoinsCountChangeEvent += setTotalCoinsText;
		GameData.playerHighestScoreChangeEvent += setHighestScoreText;
	}

	void OnDisable() {
		GameData.playerScoreChangeEvent -= setScoreText;
		GameData.playerHealthChangeEvent -= setHealthSlider;
		GameData.coinsCountChangeEvent -= setCoinsText;
		GameData.totalCoinsCountChangeEvent -= setTotalCoinsText;
		GameData.playerHighestScoreChangeEvent -= setHighestScoreText;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setScoreText(int playerScore){
		//Debug.Log("set player score called.");
		scoreText.text = playerScore.ToString ();
		endMenuScoreText.text = playerScore.ToString ();
	}

	void setHealthSlider (float playerHealth) {
		//Debug.Log("set health slider called.");
		healthSlider.value = playerHealth;
	}

	void setCoinsText (int coinsCount){
		//Debug.Log ("set coins text called.");
		coinsText.text = coinsCount.ToString ();
		endMenuCoinsText.text = coinsCount.ToString ();
	}

	void setTotalCoinsText(int totalCoinsCount){
		endMenuTotalCoinsText.text = totalCoinsCount.ToString ();
	}

	void setHighestScoreText (int playerHighestScore){
		endMenuHighestScoreText.text = playerHighestScore.ToString ();
	}
}
