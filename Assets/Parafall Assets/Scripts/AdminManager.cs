using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdminManager : MonoBehaviour {

	public GameObject adminPanel;

	private GameData gameData;

	public Text fpsText;

	public Text drawCallsText;

	private float frameCount = 0f;

	private float dt = 0f;

	private float fps = 0f;

	private float updateRate = 4f;

	// Use this for initialization
	void Start () {
		gameData = GameData.Instance;
	}
	
	// Update is called once per frame
	//void Update () {
	//	frameCount++;
	//	dt += Time.deltaTime;
	//	if (dt > 1f / updateRate) {
	//		fps = frameCount/dt;
	//		fpsText.text = fps.ToString();
	//		frameCount = 0;
	//		dt -= 1f/updateRate;
	//	}
	//}

	public void enableAdminPanel(){
		Time.timeScale = 0;
		adminPanel.SetActive (true);
	}

	public void disableAdminPanel(){
		Time.timeScale = 1;
		adminPanel.SetActive (false);
	}

	public void addGrabAllPowerUp(){
		if(null == gameData)
			gameData = GameData.Instance;
		gameData.setPowerUps ("graballpowerup", gameData.getPowerUpCount ("graballpowerup") + 1, true);
	}

	public void addSlowDownPowerUp(){
		if(null == gameData)
			gameData = GameData.Instance;
		gameData.setPowerUps ("slowdownfallpowerup", gameData.getPowerUpCount ("slowdownfallpowerup") + 1, true);
	}

	public void add1000Coins(){
		if(null == gameData)
			gameData = GameData.Instance;
		gameData.setTotalCoinsCount (gameData.getTotalCoinsCount () + 1000);
	}
}
