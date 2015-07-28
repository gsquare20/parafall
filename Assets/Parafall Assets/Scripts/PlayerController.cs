using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Text scoreText;

	public Text coinsText;
	
	public Slider healthSlider;

	void OnEnable() {
		GameData.playerScoreChangeEvent += setScoreText;
		GameData.playerHealthChangeEvent += setHealthSlider;
		GameData.coinsCountChangeEvent += setCoinsText;
	}

	void OnDisable() {
		GameData.playerScoreChangeEvent -= setScoreText;
		GameData.playerHealthChangeEvent -= setHealthSlider;
		GameData.coinsCountChangeEvent -= setCoinsText;
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
	}

	void setHealthSlider (float playerHealth) {
		//Debug.Log("set health slider called.");
		healthSlider.value = playerHealth;
		if (playerHealth == 0) {
			//TODO : Change game state to end state.		
		}
	}

	void setCoinsText (int coinsCount){
		//Debug.Log ("set coins text called.");
		coinsText.text = coinsCount.ToString ();
	}
}
