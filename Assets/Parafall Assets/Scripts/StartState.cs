using UnityEngine;
using System.Collections;

public class StartState : IGameState {

	private StateManager myStateManager;

	private ParafallObjectPool parafallObjectPool = ParafallObjectPool.Instance;

	private GameData gameData = GameData.Instance;

	public StartState(StateManager stateManager){
		myStateManager = stateManager;
	}

	public void executeStateRelatedTasks(){
		Time.timeScale = 1;
		myStateManager.mainMenu.SetActive (false);

		IGameState lastState = myStateManager.getLastState ();
		if (null != lastState && (lastState.GetType() == typeof(InitState))) {

			//Resetting object pool
			parafallObjectPool.resetObjectPool();

			//Resetting player score
			gameData.setPlayerScore(0);

			//Reset player's health
			gameData.setPlayerHealth(10f);

			//Reset coins count
			gameData.setCoinsCount(0);
		}
	}
}
