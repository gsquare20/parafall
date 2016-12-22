using UnityEngine;
using System.Collections;

public class OptionsMenuState : IGameState {

	private StateManager myStateManager;

	public OptionsMenuState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		myStateManager.optionsMenu.SetActive (true);
		myStateManager.mainMenu.SetActive (false);
		myStateManager.pauseMenu.SetActive (false);
	}
}
