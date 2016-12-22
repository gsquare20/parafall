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
		myStateManager.gamePlayPanel.SetActive (false);
		myStateManager.optionsMenu.SetActive (false);
		myStateManager.backgroundImage.SetActive (false);
		myStateManager.menuBackgroundPanel.SetActive (true);
		PowerUpManager.Instance.powerUpMenu.SetActive (false);
		AdManager.Instance.showInterstitialAd ();
		AdManager.Instance.showBannerAd ();
	}
}
