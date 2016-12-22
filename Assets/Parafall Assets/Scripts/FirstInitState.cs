using UnityEngine;
using System.Collections;

public class FirstInitState : IGameState {
	
	private StateManager myStateManager;
	
	public FirstInitState(StateManager stateManager){
		myStateManager = stateManager;
	}
	
	public void executeStateRelatedTasks(){
		
		Time.timeScale = 0;
		myStateManager.mainMenu.SetActive (true);
		myStateManager.pauseMenu.SetActive (false);
		myStateManager.gamePlayPanel.SetActive (false);
		myStateManager.backgroundImage.SetActive (false);
		myStateManager.menuBackgroundPanel.SetActive (true);
	}
}
