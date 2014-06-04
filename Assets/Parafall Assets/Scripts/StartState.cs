using UnityEngine;
using System.Collections;

public class StartState : IGameState {

	private StateManager myStateManager;

	public StartState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
	}
}
