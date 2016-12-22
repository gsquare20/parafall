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

	public GameObject liveAgainPowerPopUp;

	private static PlayerController instance;

	private bool isModalPanelActive;

	public Text playerHealthBarsCount;

	private bool isTimeScaleZero;

	private bool isTimeScaleZeroForDefaultPopUp;

	private bool isTimeScaleZeroForErrorPopUp;

	private StateManager stateManager;

	public delegate void ShowPopUpAction ();
	public static event ShowPopUpAction showPopUpEvent;

	public delegate void LoadGameDataAction ();
	public static event LoadGameDataAction loadGameDataEvent;

	public delegate void LiveAgainPowerUpNotUsedAction ();
	public static event LiveAgainPowerUpNotUsedAction liveAgainPowerUpNotUsedEvent;

	public delegate void LiveAgainPowerUpUsedAction ();
	public static event LiveAgainPowerUpUsedAction liveAgainPowerUpUsedEvent;

	void OnEnable() {
		Debug.Log ("enabling playerScoreChangeEvent in player controller.");
		GameData.playerScoreChangeEvent += setScoreText;
		GameData.playerHealthChangeEvent += setHealthSlider;
		GameData.coinsCountChangeEvent += setCoinsText;
		GameData.totalCoinsCountChangeEvent += setTotalCoinsText;
		GameData.playerHighestScoreChangeEvent += setHighestScoreText;
		GameData.playerHealthBarsCountChangeEvent += setPlayerHealthBarsText;
		StateManager.hidePopUpEvent += hideErrorPopUp;
		GameData.playerNoOfLivesChangeEvent += setNoOfLivesOnLiveAgainPowerPopUp;
		loadGameDataEvent ();
	}

	void OnDisable() {
		GameData.playerScoreChangeEvent -= setScoreText;
		GameData.playerHealthChangeEvent -= setHealthSlider;
		GameData.coinsCountChangeEvent -= setCoinsText;
		GameData.totalCoinsCountChangeEvent -= setTotalCoinsText;
		GameData.playerHighestScoreChangeEvent -= setHighestScoreText;
		GameData.playerHealthBarsCountChangeEvent -= setPlayerHealthBarsText;
		GameData.playerNoOfLivesChangeEvent += setNoOfLivesOnLiveAgainPowerPopUp;
		StateManager.hidePopUpEvent -= hideErrorPopUp;
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

	void Start(){
		stateManager = StateManager.Instance;	
	}


	void setScoreText(int playerScore){
		Debug.Log("set player score called.");
		scoreText.text = playerScore.ToString ();
		endMenuScoreText.text = playerScore.ToString ();
	}

	void setHealthSlider (float playerHealth) {
		//Debug.Log("set health slider called.");
		healthSlider.value = playerHealth;
		if (playerHealth == 0f) {
			showLiveAgainPowerPopUp();		
		}
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

	void setPlayerHealthBarsText(int playerHealthBars){
		playerHealthBarsCount.text = playerHealthBars.ToString ();
	}

	public void toggleSoundInGame(){
		if(AudioListener.volume == 0f)
			AudioListener.volume = 1f;
		else
			AudioListener.volume = 0f;

		Debug.Log ("Audio Listener volume : " + AudioListener.volume);
	}

	public void showErrorPopUp(string errorMsg){
		if(modalPanel.activeSelf)
			isModalPanelActive = true;
		else{
			modalPanel.SetActive (true);
			isModalPanelActive = false;
		}
		errorPopUp.SetActive (true);
		errorMsgText.text = errorMsg;

		if (Time.timeScale == 0f) {
			isTimeScaleZeroForErrorPopUp = true;		
		}else{
			isTimeScaleZeroForErrorPopUp = false;
			Time.timeScale = 0f;
		}

		AdManager.Instance.showBannerAd ();
	}

	public void hideErrorPopUp(){

		errorPopUp.SetActive (false);
		if(!isModalPanelActive)
			modalPanel.SetActive (false);

		if(!isTimeScaleZeroForErrorPopUp){
			Time.timeScale = 1f;
		}

		AdManager.Instance.hideBannerAd ();
	}

	public void showDefaultPopUp(string msg){
		if(modalPanel.activeSelf)
			isModalPanelActive = true;
		else{
			modalPanel.SetActive (true);
			isModalPanelActive = false;
		}
		defaultPopUp.SetActive (true);
		defaultMsgText.text = msg;

		if (Time.timeScale == 0f) {
			isTimeScaleZeroForDefaultPopUp = true;		
		}else{
			isTimeScaleZeroForDefaultPopUp = false;
			Time.timeScale = 0f;
		}

		AdManager.Instance.showBannerAd ();
	}

	public void hideDefaultPopUp(){

		defaultPopUp.SetActive (false);
		if(!isModalPanelActive)
			modalPanel.SetActive (false);

		if(!isTimeScaleZeroForDefaultPopUp){
			Time.timeScale = 1f;
		}

		AdManager.Instance.hideBannerAd ();
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

		if (Time.timeScale == 0f) {
			isTimeScaleZero = true;		
		}else{
			isTimeScaleZero = false;
			Time.timeScale = 0f;
			stateManager.gamePlayPanel.SetActive (false);
		}

		showPopUpEvent ();

		AdManager.Instance.showBannerAd ();
	}

	public void hideShopAndBoostUpPopUp(){
		modalPanel.SetActive (false);
		shopAndBoostUpPopUp.SetActive (false);

		if(!isTimeScaleZero){
			Time.timeScale = 1f;
			stateManager.gamePlayPanel.SetActive (true);
		}

		AdManager.Instance.hideBannerAd ();
	}

	public void showLiveAgainPowerPopUp(){
		if(GameData.Instance.getNoOfLives() > 0){
			modalPanel.SetActive (true);
			liveAgainPowerPopUp.SetActive (true);
			Time.timeScale = 0f;
		}
		else{
			liveAgainPowerUpNotUsedEvent();
		}

		AdManager.Instance.showBannerAd ();
	}

	public void hideLiveAgainPowerPopUp(bool okBtnClicked){
		modalPanel.SetActive (false);
		liveAgainPowerPopUp.SetActive (false);
		if (!okBtnClicked) {
			liveAgainPowerUpNotUsedEvent();	
		}else{
			liveAgainPowerUpUsedEvent();
			Time.timeScale = 1f;
		}

		AdManager.Instance.hideBannerAd ();
	}

	private void setNoOfLivesOnLiveAgainPowerPopUp(int noOfLives){
		liveAgainPowerPopUp.transform.GetChild (1).GetChild (0).GetComponent<Text>().text = noOfLives.ToString ();
	}
}
