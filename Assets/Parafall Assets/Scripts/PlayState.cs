using UnityEngine;
using System.Collections;

public class PlayState : IGameState {

	private StateManager myStateManager;

	public PlayState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
	}
}