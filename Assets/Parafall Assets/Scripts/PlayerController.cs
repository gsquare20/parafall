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

	public GameObject modalPanel;

	public GameObject errorPopUp;

	public GameObject defaultPopUp;

	public Text errorMsgText;

	public Text defaultMsgText;

	public Text testText;

	public GameObject shopAndBoostUpPopUp;

	private static PlayerController instance;

	void OnEnable() {
		Debug.Log ("enabling playerScoreChangeEvent in player controller.");
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

	public static PlayerController Instance {
		get{
			if(null == instance){
				instance = GameObject.Find ("Player").GetComponent<PlayerController>();
			}
			
			return instance;
		}
	}

	// Use this for initialization
	void Awake(){
		instance = this;
	}


	void setScoreText(int playerScore){
		Debug.Log("set player score called.");
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

	public void toggleSoundInGame(){
		if(AudioListener.volume == 0f)
			AudioListener.volume = 1f;
		else
			AudioListener.volume = 0f;

		Debug.Log ("Audio Listener volume : " + AudioListener.volume);
	}

	public void showErrorPopUp(string errorMsg){
		modalPanel.SetActive (true);
		errorPopUp.SetActive (true);
		errorMsgText.text = errorMsg;
	}

	public void hideErrorPopUp(){
		modalPanel.SetActive (false);
		errorPopUp.SetActive (false);
	}

	public void showDefaultPopUp(string msg){
		modalPanel.SetActive (true);
		defaultPopUp.SetActive (true);
		defaultMsgText.text = msg;
	}

	public void hideDefaultPopUp(){
		modalPanel.SetActive (false);
		defaultPopUp.SetActive (false);
	}

	public void showShopAndBoostUpPopUp(string tabName){
		modalPanel.SetActive (true);
		shopAndBoostUpPopUp.SetActive (true);
		switch(tabName){
		case "shop":
			shopAndBoostUpPopUp.transform.FindChild("Shop Sub Panel").SetAsLastSibling();
			break;
		case "boostup":
			shopAndBoostUpPopUp.transform.FindChild("BoostUp Sub Panel").SetAsLastSibling();
			break;
		}
	}

	public void hideShopAndBoostUpPopUp(){
		modalPanel.SetActive (false);
		shopAndBoostUpPopUp.SetActive (false);
	}
}
