using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ParafallEventManager : MonoBehaviour {
	
	public List<string> playerScoreAndParachuteSpeedList = new List<string>();
	
	public delegate void PlayerScoreAndParachuteSpeedAction (float fallSpeed);
	public static event PlayerScoreAndParachuteSpeedAction playerScoreAndParachuteSpeedEvent;
	
	private GameData gameData;
	
	// Use this for initialization
	void Start () {
		gameData = GameData.Instance;
		InvokeRepeating ("callPlayerScoreAndParachuteSpeedLogic", 5f, 2f);
	}
	
	// Update is called once per frame
	void callPlayerScoreAndParachuteSpeedLogic () {
		foreach (string playerScoreAndParachuteSpeed in playerScoreAndParachuteSpeedList) {
			string[] strSeperators = {","};
			string[] playerScoreAndParachuteSpeedArr = playerScoreAndParachuteSpeed.Split (strSeperators, StringSplitOptions.None);
			int playerScore = IntParseFast(playerScoreAndParachuteSpeedArr[0]);
			int parachuteSpeed = IntParseFast (playerScoreAndParachuteSpeedArr[1]);
			int parachuteNo = IntParseFast(playerScoreAndParachuteSpeedArr[2]);
			if(gameData.getPlayerScore() == playerScore){
				GameObject.Find ("Parachute Spawners").GetComponentsInChildren<ParachuteSpawner>()[parachuteNo-1].fallSpeed = parachuteSpeed;
				//Debug.Log ("Speed increased");
				break;
			}
		}
	}
	
	void callPlayerScoreAndParachuteSpeedLogic000(){
		if (gameData.getPlayerScore () == 10) {
			if (null != playerScoreAndParachuteSpeedEvent){
				playerScoreAndParachuteSpeedEvent (200f);
			}
		}
	}
	
	public static int IntParseFast(string value){
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}
}