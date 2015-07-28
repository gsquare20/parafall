using UnityEngine;
using System.Collections;

public class InitState : IGameState {

	private StateManager myStateManager;

	public InitState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){

		myStateManager.mainMenu.SetActive (true);

		myStateManager.pauseMenu.SetActive (false);

		Time.timeScale = 0;
	}
}
