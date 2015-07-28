using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	private IGameState activeState;

	private IGameState lastState;

	private static StateManager instance;

	public GameObject mainMenu;

	public GameObject pauseMenu;

	private Dictionary<int, IGameState> gameStatesDict = new Dictionary<int, IGameState>();

	public delegate void StartSpawningParaPacketsAction ();
	public static event StartSpawningParaPacketsAction startSpawningParaPacketsEvent;

	public enum GameStateEnum {
		InitState,
		StartState,
		PauseState,
		ResumeState,
		OptionsMenuState,
		EndState,
		ExitState
	}

	void Awake(){
		if(null == instance){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			DestroyImmediate(gameObject);

	}

	// Use this for initialization
	void Start () {
		gameStatesDict.Add ((int)GameStateEnum.InitState, new InitState (this));
		gameStatesDict.Add ((int)GameStateEnum.StartState, new StartState (this));
		gameStatesDict.Add ((int)GameStateEnum.PauseState, new PauseState (this));
		gameStatesDict.Add ((int)GameStateEnum.ResumeState, new ResumeState (this));
		gameStatesDict.Add ((int)GameStateEnum.OptionsMenuState, new OptionsMenuState (this));
		gameStatesDict.Add ((int)GameStateEnum.EndState, new EndState (this));
		gameStatesDict.Add ((int)GameStateEnum.ExitState, new ExitState (this));

		Time.timeScale = 0;
		mainMenu.SetActive (true);
		pauseMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//if(null != activeState)
		//	activeState.executeStateRelatedTasks();
		if (Input.GetKeyDown (KeyCode.Escape)) {
			System.Type currentStateType = activeState.GetType();
			if(currentStateType == typeof(StartState) || currentStateType == typeof(ResumeState)){
				pauseGame();
			}
			if(currentStateType == typeof(InitState) || currentStateType == typeof(PauseState)){
				Application.Quit();	
			}
		}
	}

	public void switchState(IGameState newState){
		activeState = newState;
		activeState.executeStateRelatedTasks ();
	}

	public void playGame(){
		setLastState ();
		//switchState (new StartState (this));
		switchState (getGameState(GameStateEnum.StartState));
		startSpawningParaPacketsEvent ();
	}

	public void pauseGame(){
		setLastState ();
		//switchState (new PauseState (this));
		switchState (getGameState(GameStateEnum.PauseState));
	}

	public void exitGame(){
		setLastState ();
		//switchState (new ExitState (this));
		switchState (getGameState(GameStateEnum.ExitState));
	}

	public void openOptionsMenu() {
		setLastState ();
		//switchState (new OptionsMenuState (this));
		switchState (getGameState(GameStateEnum.OptionsMenuState));
	}

	public void resumeGame() {
		setLastState ();
		//switchState (new ResumeState (this));
		switchState (getGameState(GameStateEnum.ResumeState));
	}

	public void goBackToMainMenu() {
		setLastState ();
		//switchState (new InitState (this));
		switchState (getGameState(GameStateEnum.InitState));
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
}
