using UnityEngine;
using System.Collections;

public class EndState : IGameState {

	private StateManager myStateManager;

	public EndState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		//TODO : Show End state menu showing final score, coins and social share options.
		Time.timeScale = 0;
		myStateManager.endMenu.SetActive (true);
	}
}