using UnityEngine;
using System.Collections;

public class ExitState : IGameState {

	private StateManager myStateManager;

	public ExitState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		Application.Quit ();
	}
}
