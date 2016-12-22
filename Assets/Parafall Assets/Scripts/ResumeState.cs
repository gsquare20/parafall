using UnityEngine;
using System.Collections;

public class ResumeState : IGameState {

	private StateManager myStateManager;

	public ResumeState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		Time.timeScale = 1;
		myStateManager.pauseMenu.SetActive (false);
		myStateManager.gamePlayPanel.SetActive (true);
		myStateManager.backgroundImage.SetActive (true);
		myStateManager.menuBackgroundPanel.SetActive (false);
	}
}
