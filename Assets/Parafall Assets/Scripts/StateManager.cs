using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

	private IGameState activeState;

	private static StateManager instance;

	void Awake(){
		if(null == instance){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			DestroyImmediate(gameObject);

	}

	// Use this for initialization
	void Start () {
		activeState = new StartState(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(null != activeState)
			activeState.executeStateRelatedTasks();
	}

	public void switchState(IGameState newState){
		activeState = newState;
	}
}
