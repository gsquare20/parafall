using UnityEngine;
using System.Collections;

public class PauseState : IGameState {

	private StateManager myStateManager;

	public PauseState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		Time.timeScale = 0;
		myStateManager.pauseMenu.SetActive (true);
	}
}
