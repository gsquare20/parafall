using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	private IGameState activeState;

	private IGameState lastState;

	private static StateManager instance;

	public GameObject mainMenu;

	public GameObject pauseMenu;

	public GameObject endMenu;

	public GameObject optionsMenu;

	public GameObject gamePlayPanel;

	public GameObject backgroundImage;

	public GameObject menuBackgroundPanel;

	private Dictionary<int, IGameState> gameStatesDict = new Dictionary<int, IGameState>();

	public delegate void StartSpawningParaPacketsAction ();
	public static event StartSpawningParaPacketsAction startSpawningParaPacketsEvent;

	public delegate void InitStateAction ();
	public static event InitStateAction initStateEvent;

	public delegate void EndStateAction ();
	public static event EndStateAction endStateEvent;

	public delegate void HidePopUpAction ();
	public static event HidePopUpAction hidePopUpEvent;

	public enum GameStateEnum {
		FirstInitState,
		InitState,
		StartState,
		PauseState,
		ResumeState,
		OptionsMenuState,
		EndState,
		ExitState,
		PopUpState
	}

	void Awake(){
		if(null != instance)
			DestroyImmediate(gameObject);
		else
			instance = this;

	}

	public static StateManager Instance {
		get{
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<StateManager>();
			}
			
			return instance;
		}
	}

	void OnEnable(){
		GameData.playerHealthChangeEvent += moveToEndStateBasedOnPlayerHealth;
		PlayerController.showPopUpEvent += changeGameStateOnShowPopUp;
		PlayerController.liveAgainPowerUpNotUsedEvent += endGame;
	}

	void OnDisable() {
		GameData.playerHealthChangeEvent -= moveToEndStateBasedOnPlayerHealth;
		PlayerController.showPopUpEvent -= changeGameStateOnShowPopUp;
		PlayerController.liveAgainPowerUpNotUsedEvent -= endGame;
	}

	// Use this for initialization
	void Start () {
		gameStatesDict.Add ((int)GameStateEnum.FirstInitState, new FirstInitState (this));
		gameStatesDict.Add ((int)GameStateEnum.InitState, new InitState (this));
		gameStatesDict.Add ((int)GameStateEnum.StartState, new StartState (this));
		gameStatesDict.Add ((int)GameStateEnum.PauseState, new PauseState (this));
		gameStatesDict.Add ((int)GameStateEnum.ResumeState, new ResumeState (this));
		gameStatesDict.Add ((int)GameStateEnum.OptionsMenuState, new OptionsMenuState (this));
		gameStatesDict.Add ((int)GameStateEnum.EndState, new EndState (this));
		gameStatesDict.Add ((int)GameStateEnum.ExitState, new ExitState (this));
		gameStatesDict.Add ((int)GameStateEnum.PopUpState, new PopUpState (this));

		Time.timeScale = 0;
		mainMenu.SetActive (true);
		pauseMenu.SetActive (false);
		gamePlayPanel.SetActive (false);
		backgroundImage.SetActive (false);
		menuBackgroundPanel.SetActive (true);

	}
	
	// Update is called once per frame
	void Update () {
		//if(null != activeState)
		//	activeState.executeStateRelatedTasks();
		if (Input.GetKeyDown (KeyCode.Escape)) {
			System.Type currentStateType = typeof(InitState);
			System.Type lastStateType = typeof(InitState);

			if(null != activeState)
				currentStateType = activeState.GetType();

			if(null != lastState)
				lastStateType = lastState.GetType ();

			if(currentStateType == typeof(StartState) || currentStateType == typeof(ResumeState)){
				pauseGame();
			}

			if(currentStateType == typeof(PauseState)){
				resumeGame();
			}

			if(currentStateType == typeof(InitState)){
				Application.Quit();	
			}

			if(currentStateType == typeof(OptionsMenuState) && lastStateType == typeof(PauseState)){
				pauseGame();
			}
			else if (currentStateType == typeof(OptionsMenuState)){
				goBackToMainMenu();	
			}

			if(currentStateType == typeof(PopUpState) && lastStateType == typeof(PauseState)){
				pauseGame();
				hidePopUpEvent();
			}
			else if (currentStateType == typeof(PopUpState) && (lastStateType != typeof(StartState) && lastStateType != typeof(ResumeState))){
				goBackToMainMenu();
				hidePopUpEvent();
			}
		}
	}

	public void switchState(IGameState newState){
		AdManager.Instance.hideBannerAd ();
		setLastState ();
		activeState = newState;
		activeState.executeStateRelatedTasks ();
	}

	public void playGame(){
		//switchState (new StartState (this));
		switchState (getGameState(GameStateEnum.StartState));
		startSpawningParaPacketsEvent ();
	}

	public void pauseGame(){
		//switchState (new PauseState (this));
		switchState (getGameState(GameStateEnum.PauseState));
	}

	public void endGame(){
		endStateEvent ();

		switchState (getGameState (GameStateEnum.EndState));
	}

	public void exitGame(){
		//switchState (new ExitState (this));
		switchState (getGameState(GameStateEnum.ExitState));
	}

	public void openOptionsMenu() {
		//switchState (new OptionsMenuState (this));
		switchState (getGameState(GameStateEnum.OptionsMenuState));
	}

	public void resumeGame() {
		//switchState (new ResumeState (this));
		switchState (getGameState(GameStateEnum.ResumeState));
	}

	public void goBackToMainMenu() {
		initStateEvent ();

		//switchState (new InitState (this));
		switchState (getGameState(GameStateEnum.InitState));
	}

	public void goBackFromOptionsMenu(){
		if(lastState != null){
			System.Type lastStateType = lastState.GetType ();
			if(lastStateType == typeof(PauseState)){
				pauseGame();
			}else{
				goBackToMainMenu();
				}
		}else{
			goBackToMainMenu();
		}
	}

	private void setLastState(){
		lastState = activeState;
	}

	public IGameState getLastState(){
		return lastState;
	}

	public IGameState getGameState(GameStateEnum gameStateEnum){
		int state = (int)gameStateEnum;
		return gameStatesDict[state];
	}

	void moveToEndStateBasedOnPlayerHealth(float playerHealth){
		if (playerHealth == 0f) {
			//endGame ();		
		}
	}

	void changeGameStateOnShowPopUp(){
		switchState (getGameState(GameStateEnum.PopUpState));
	}
}
