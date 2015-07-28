using UnityEngine;
using System.Collections;

public class OptionsMenuState : IGameState {

	private StateManager myStateManager;

	public OptionsMenuState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		//TODO : Need to open options dialog.
	}
}
