using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour {

	public GameObject adminPanel;

	private GameData gameData;

	// Use this for initialization
	void Start () {
		gameData = GameData.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void enableAdminPanel(){
		Time.timeScale = 0;
		adminPanel.SetActive (true);
	}

	public void disableAdminPanel(){
		Time.timeScale = 1;
		adminPanel.SetActive (false);
	}

	public void addGrabAllPowerUp(){
		gameData.setPowerUps ("graballpowerup", gameData.getPowerUpCount ("graballpowerup") + 1, true);
	}

	public void addSlowDownPowerUp(){
		gameData.setPowerUps ("slowdownfallpowerup", gameData.getPowerUpCount ("slowdownfallpowerup") + 1, true);
	}
}
