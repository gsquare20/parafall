using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour {

	private InterstitialAd interstitialAd;

	private AdRequest request;

	private static AdManager instance;

	public static AdManager Instance {
		get {
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<AdManager>();
			}
			
			return instance;
		}
	}

	void Awake(){
		if(null != instance)
			DestroyImmediate(gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
		requestInterstitialAd ();
		InvokeRepeating ("loadInterstitialAd", 30f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable(){
		interstitialAd.Destroy ();
	}

	private void requestBannerAd(){
	}

	private void requestInterstitialAd(){

		//string adUnitID = "ca-app-pub-5639974140498794/5510892664";
		#if UNITY_ANDROID
			string adUnitID = "ca-app-pub-5639974140498794/5510892664";
		#else
			string adUnitID = "ca-app-pub-5639974140498794/5510892664";
		#endif

		interstitialAd = new InterstitialAd(adUnitID);
		request = new AdRequest.Builder().Build();

		interstitialAd.LoadAd(request);
		interstitialAd.AdLoaded += handleAdLoaded;
		interstitialAd.AdFailedToLoad += handleAdFailedToLoad;
		interstitialAd.AdOpened += handleAdOpened;
		interstitialAd.AdClosing += handleAdClosing;
		interstitialAd.AdClosed += handleAdClosed;
	}

	private void loadInterstitialAd(){
		interstitialAd.LoadAd (request);
	}

	void handleAdClosed (object sender, EventArgs e)
	{
		Debug.Log ("Ad Closed!");
	}

	void handleAdClosing (object sender, EventArgs e)
	{
		Debug.Log ("Ad Closing!");
	}

	public void handleAdFailedToLoad(object sender, EventArgs args){
		Debug.Log ("Ad Failed To Load.");
	}
	
	public void showInterstitialAd(){
		if (interstitialAd.IsLoaded ())
			interstitialAd.Show ();
		else{
			loadInterstitialAd();
			interstitialAd.Show ();
		}
	}

	private void requestCustomSizeAd(){
	}

	public void handleAdLoaded(object sender, EventArgs args){
		Debug.Log ("Ad Loaded.");
	}

	public void handleAdOpened(object sender, EventArgs args){
		Debug.Log ("Ad Opened.");
	}
}
