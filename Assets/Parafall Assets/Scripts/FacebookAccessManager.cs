using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;

public class FacebookAccessManager : MonoBehaviour {

	public Text FBButtonText;

	public GameObject FBAvatarGO;

	public GameObject scoreTextGOToInstantiate;

	public GameObject scoreListGO;

	public GameObject fbMaskPanelGO;

	private int myGameScore;

	private bool isGameScoreRetrieved;

	private PlayerController playerController;

	// Use this for initialization
	void Awake () {
		FB.Init (onInitComplete, onHideUnity, null);
		isGameScoreRetrieved = false;
		//FBButtonText = gameObject.GetComponentInChildren<Text>();
		playerController = PlayerController.Instance;
	}

	void OnEnable(){
		//StateManager.endStateEvent += setAndGetScores;
		GameData.playerHighestScoreChangeEvent += getHighestScore;
	}

	void OnDisable(){
		//StateManager.endStateEvent -= setAndGetScores;
		GameData.playerHighestScoreChangeEvent -= getHighestScore;
	}

	private bool isFacebookLoginSuccessful = false;
	private bool isFacebookInitializationDone = false;

	public void connectToFB(){
		if (isFacebookLoginSuccessful)
			logoutFromFB ();
		else{
			if(isFacebookInitializationDone)
				loginToFB();
		}	
	}

	public void connectFBOnEndMenu(){
		connectToFB ();

	}

	private void loginToFB(){
		//FBButtonText.text = "Connecting...";
		if(Util.isNetworkConnectionAvailable())
			FB.Login ("email,publish_actions", fbLoginCallback);
		else
			playerController.showErrorPopUp ("No internet connection available!");
	}

	private void logoutFromFB(){
		FB.Logout ();
		FBButtonText.text = "LOG IN TO FACEBOOK";
		isFacebookLoginSuccessful = false;
		//FBAvatarGO.SetActive (false);
		FBAvatarGO.GetComponent<Image> ().sprite = null;
		fbMaskPanelGO.SetActive (true);
		Debug.Log ("User successfully logged out!");
	}

	private void fbLoginCallback(FBResult result){
		if (FB.IsLoggedIn) {
			onLoggedIn ();
		}
		else{
			onNotLoggedIn();
		}
	}

	private void onLoggedIn(){
		isFacebookLoginSuccessful = true;
		FBButtonText.text = "LOGOUT FROM FACEBOOK";
		retrieveRequiredFBData();
		fbMaskPanelGO.SetActive (false);
		setScore ();
		Debug.Log ("User successfully logged in!");
	}

	private void onNotLoggedIn(){
		playerController.showErrorPopUp ("Could not be able to connect to Facebook.");
		Debug.Log ("User could not able to log in.");
	}

	private void onInitComplete(){
		//playerController.showDefaultPopUp ("FB Init done!");
		isFacebookInitializationDone = true;
		Debug.Log ("FB Init done!");

		if (FB.IsLoggedIn) {
			onLoggedIn ();		
		}
	}

	private void onHideUnity(bool isGameActive){
		playerController.showDefaultPopUp ("Is game active : " + isGameActive.ToString ());
	}

	private void retrieveRequiredFBData(){
		//TODO : Take reference from https://www.youtube.com/watch?v=9H8Wc8NRP-I&feature=iv&src_vid=nmeMHY5D4SI&annotation_id=annotation_408344375

		//Get profile picture
		//playerController.showDefaultPopUp("Sending request to retrieve profile picture.");
		FB.API (Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, getPictureApiCallback);

		//onFBFeed ();

		//Get profile name
		//FB.API ("/me?fields=id,first_name", Facebook.HttpMethod.GET, getProfileNameCallback);

		//Query Scores
		//FB.API ("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, getScoresCallback);

		//Set Scores
		//FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result) 	{

		//}, new Dictionary<string, string>());
	}

	private void onFBFeed(){
		if(FB.IsLoggedIn){
			FB.Feed (
				link: "http://apps.facebook.com/" + FB.AppId,
				linkName: "Checkout my Parafall score. I did a pretty good job.",
				linkCaption: "My Score is 000.",
				picture: "http://greyzoned.com/images/evilelf2_icon.png"
				);
		}
	}

	private void onFBBeatMe(){
		if (FB.IsLoggedIn) {
			FB.AppRequest(
					"This game is awesome, join me. Now.",
					null,
					null,
					null,
					null,
					"",
					"Invite your friends to join you.",
					beatMeCallback
				);
		}
	}

	private void getPictureApiCallback(FBResult result){
		if(result.Error == null){
			//playerController.showDefaultPopUp("Retrieving profile picture.");
			Texture2D FBAvatarTexture = result.Texture;
			Image FBAvatar = FBAvatarGO.GetComponent<Image> ();
			FBAvatar.sprite = Sprite.Create (FBAvatarTexture, new Rect (0, 0, 128, 128), new Vector2 (0, 0));
			//FBAvatarGO.SetActive (true);
		}
		else{
			playerController.showErrorPopUp(result.Error.ToString());
		}


	}

	private void beatMeCallback(FBResult result){
		if (result.Error != null)
			Debug.Log (result.Error);

		Debug.Log (result.Text.ToString ());
	}

	public void onFBFeedClick(){
		onFBFeed ();
	}

	public void onFBBeatMeClick(){
		onFBBeatMe ();
	}

	private void setAndGetScores(){
		
	}

	private void setScore(){
		if (FB.IsLoggedIn && isGameScoreRetrieved) {
			Dictionary<string, string> scoreData = new Dictionary<string, string>();
			scoreData["score"] = myGameScore.ToString();
			FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result) 	{
					if(result.Error != null){
						Debug.Log("Could not set score in Facebook.");
					playerController.showErrorPopUp("Could not set score in Facebook.");
					}
					Debug.Log ("FB Score submit status : " + result.Text);
					deleteExistingScoreItems();
					getScore ();
				}
			, scoreData);		
		}
	}

	private void deleteExistingScoreItems(){
		int childCount = scoreListGO.transform.childCount;
		for (int i=0; i<childCount; i++) {
			GameObject.Destroy(scoreListGO.transform.GetChild(i).gameObject);		
		}
	}

	private void getScore(){
		if (FB.IsLoggedIn) {
			FB.API ("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, delegate(FBResult result) {
				if(result.Error != null){
					Debug.Log("Could not get scores from Facebook.");
					playerController.showErrorPopUp("Could not get scores from Facebook.");
				}
				List<object> scoreList = Util.DeserializeScores(result.Text);
				foreach(object score in scoreList){
					var entry = (Dictionary<string, object>) score;
					var user = (Dictionary<string, object>) entry["user"];



					GameObject scoreTextGO = (GameObject)Instantiate(scoreTextGOToInstantiate);
					Text[] scoreTextGOArr = scoreTextGO.GetComponentsInChildren<Text>();
					scoreTextGOArr[0].text = user["name"].ToString().ToUpper();
					scoreTextGOArr[1].text = entry["score"].ToString();

					scoreTextGO.transform.SetParent (scoreListGO.transform);
				}
			});
	
		}
	}

	private void getHighestScore(int playerHighestScore){
		Debug.Log ("Get highest score method called in Facebook Access Manager.");
		myGameScore = playerHighestScore;
		isGameScoreRetrieved = true;
		setScore ();
		//getScore ();
	}
}
