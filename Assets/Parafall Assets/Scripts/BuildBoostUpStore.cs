using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuildBoostUpStore : MonoBehaviour {

	public BoostUpStore boostUpStore;

	public GameObject boostUpItem;

	public GameObject boostUpItemsList;

	private Dictionary<string, BoostUpObject> boostUpStoreDict = new Dictionary<string, BoostUpObject>();

	private GameData gameData;

	private PlayerController playerController;

	// Use this for initialization
	void Start () {
		foreach (BoostUpObject boostUpObj in boostUpStore.boostUpObjList) {
			//Put into boost up store dict
			boostUpStoreDict.Add (boostUpObj.id, boostUpObj);

			//Instantiate boost up item prefab
			GameObject boostUpItemGO = (GameObject) Instantiate (boostUpItem);
			Transform boostUpItemGOTransform = boostUpItemGO.transform;
			Debug.Log ("Boost ID while setting up : " + boostUpObj.id);
			boostUpItemGOTransform.GetChild (0).GetComponent<Text>().text = boostUpObj.title;
			boostUpItemGOTransform.GetChild (1).GetComponent<Text>().text = boostUpObj.desc;
			switch(boostUpObj.id){
			case "boost-grab-all-power-up":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("boost-grab-all-power-up"));
				break;
			case "boost-slow-down-power-up":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("boost-slow-down-power-up"));
				break;
			case "boost-double-score-power-up":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("boost-double-score-power-up"));
				break;
			case "boost-double-coins-power-up":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("boost-double-coins-power-up"));
				break;
			case "health-bar":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("health-bar"));
				break;
			case "live-again-power-up":
				boostUpItemGOTransform.GetChild (2).GetComponent<Button>().onClick.AddListener(() => buyBoostUp("live-again-power-up"));
				break;

			}


			//boostUpItemGOTransform.GetChild (3).GetComponent<Text>().text = boostUpObj.id;

			boostUpItemGOTransform.SetParent(boostUpItemsList.transform);
		}

		gameData = GameData.Instance;
		playerController = PlayerController.Instance;
	}
	
	public void buyBoostUp(string boostUpID){
		Debug.Log ("Buying Boost Up called : " + boostUpID);
		switch (boostUpID) {
		case "boost-grab-all-power-up":
			if(isBoostUpBuyValid(boostUpID)){
			gameData.setPowerUps("graballpowerup", gameData.getPowerUpCount("graballpowerup") + 1, true);
			playerController.showDefaultPopUp("Grab All Power Up Count : " + gameData.getPowerUpCount("graballpowerup").ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		case "boost-slow-down-power-up":
			if(isBoostUpBuyValid(boostUpID)){
			gameData.setPowerUps("slowdownfallpowerup", gameData.getPowerUpCount("slowdownfallpowerup") + 1, true);
				playerController.showDefaultPopUp("Slow Down Power Up Count : " + gameData.getPowerUpCount("slowdownfallpowerup").ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		case "boost-double-score-power-up":
			if(isBoostUpBuyValid(boostUpID)){
			gameData.setPowerUps("doublethescorepowerup", gameData.getPowerUpCount("doublethescorepowerup") + 1, true);
				playerController.showDefaultPopUp("Double Score Power Up Count : " + gameData.getPowerUpCount("doublethescorepowerup").ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		case "boost-double-coins-power-up":
			if(isBoostUpBuyValid(boostUpID)){
			gameData.setPowerUps("doublethecoinpowerup", gameData.getPowerUpCount("doublethecoinpowerup") + 1, true);
				playerController.showDefaultPopUp("Double Coin Power Up Count : " + gameData.getPowerUpCount("doublethecoinpowerup").ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		case "health-bar":
			if(isBoostUpBuyValid(boostUpID)){
				gameData.setPlayerHealthBars(gameData.getPlayerHealthBarsCount() + 1);
				playerController.showDefaultPopUp("Health Bar Count : " + gameData.getPlayerHealthBarsCount().ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		case "live-again-power-up":
			if(isBoostUpBuyValid(boostUpID)){
				gameData.setNoOfLives(gameData.getNoOfLives() + 1);
				playerController.showDefaultPopUp("Live Again Power Up Count : " + gameData.getNoOfLives().ToString() + "\n" + 
				                                  "Coins Left : " + gameData.getTotalCoinsCount().ToString());
			}
			else{
				playerController.showErrorPopUp("Not enough coins to buy this!");
			}
			break;
		}
	}

	private bool isBoostUpBuyValid(string boostUpID){
		int playerTotalCoinsCount = gameData.getTotalCoinsCount ();

		if (null != boostUpStoreDict && boostUpStoreDict.ContainsKey (boostUpID)) {
			BoostUpObject boostUpObj = boostUpStoreDict[boostUpID];
			if(playerTotalCoinsCount >= boostUpObj.coinValue){
				gameData.setTotalCoinsCount(playerTotalCoinsCount - boostUpObj.coinValue);
				return true;
			}
		}

		return false;
	}
}
