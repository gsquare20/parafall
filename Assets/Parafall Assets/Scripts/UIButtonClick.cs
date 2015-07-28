﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonClick : MonoBehaviour {

	public Text testText;

	private InputManager inputManager;

	private GameData gameData;

	void Start () {
		//parafallObjectPool = ParafallObjectPool.Instance;
		//inputManager = GameObject.Find ("GameManager").GetComponent<InputManager>();
		inputManager = InputManager.Instance;
		gameData = GameData.Instance;
	}

	public void onButtonClick(string buttonText){
		//Debug.Log (buttonText + " button clicked.");
		if(!buttonText.Equals ("GRAB")){
			if(testText.text.Equals("FOUND") || testText.text.Equals ("NOT FOUND"))
				testText.text = buttonText;
			else
				testText.text = testText.text + buttonText;
			string inputStr = inputManager.appendInputString(buttonText);
			//Debug.Log ("input str : " + inputStr);
		}
		

		if(buttonText.Equals("GRAB"))
		{
			//Debug.Log ("Grab button clicked.");
			ParaPacket paraPacket = inputManager.findAndGrab();
			if(null != paraPacket){
				testText.text = "FOUND";
				//Increment player Score by 1
				if(paraPacket.paraName.Equals("foodpacket"))
					gameData.setPlayerScore(gameData.getPlayerScore() + 1);

				//Increment coins count by 10
				if(paraPacket.paraName.Equals("coinpacket"))
					gameData.setCoinsCount(gameData.getCoinsCount() + 10);

				//Increment player health by 5
				if(paraPacket.paraName.Equals("healthpacket"))
				{
					if(gameData.getPlayerHealth () <= 5)
						gameData.setPlayerHealth(gameData.getPlayerHealth() + 5);
					else
						gameData.setPlayerHealth (10f);
				}

				//Incrementing player power ups
				if(paraPacket.paraName.Contains("powerup")){
					string paraPacketName = paraPacket.paraName;
					string paraPacketTrimmedName = paraPacketName.Substring(0, paraPacketName.IndexOf("packet"));
					//Debug.Log ("para packet trimmed name : " + paraPacketTrimmedName);
					gameData.setPowerUps(paraPacketTrimmedName, gameData.getPowerUpCount(paraPacketTrimmedName) + 1);
				}
			}
			else{
				testText.text = "NOT FOUND";

			}
		}
	}
}