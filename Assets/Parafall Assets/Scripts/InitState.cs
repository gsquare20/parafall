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

		myStateManager.optionsMenu.SetActive (false);

		myStateManager.endMenu.SetActive (false);

		myStateManager.gamePlayPanel.SetActive (false);

		myStateManager.backgroundImage.SetActive (false);

		myStateManager.menuBackgroundPanel.SetActive (true);

		AdManager.Instance.showBannerAd ();

		Time.timeScale = 0;
	}
}
