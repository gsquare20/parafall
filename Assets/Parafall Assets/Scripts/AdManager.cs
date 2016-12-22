using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour {

	private InterstitialAd interstitialAd;

	private BannerView bannerAd;

	private AdRequest request;

	private AdRequest bannerAdRequest;

	private static AdManager instance;

	private PlayerController playerController;

	private bool bannerAdLoaded = false;

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
		playerController = PlayerController.Instance;
		requestInterstitialAd ();
		requestBannerAd ();
		InvokeRepeating ("loadInterstitialAd", 5f, 15f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable(){
		interstitialAd.Destroy ();
		bannerAd.Destroy ();
	}

	private void requestBannerAd(){
		#if UNITY_ANDROID
			string adUnitID = "ca-app-pub-5639974140498794/4034159467";
		#else
			string adUnitID = "ca-app-pub-5639974140498794/4034159467";
		#endif

		bannerAd = new BannerView (adUnitID, AdSize.Banner, AdPosition.Bottom);
		bannerAdRequest = new AdRequest.Builder ().Build ();
		bannerAd.LoadAd (bannerAdRequest);

		bannerAd.AdLoaded += handleBannerAdLoaded;
		bannerAd.AdFailedToLoad += handleBannerAdFailedToLoad;
		bannerAd.AdOpened += handleBannerAdOpened;
		bannerAd.AdClosing += handleBannerAdClosing;
		bannerAd.AdClosed += handleBannerAdClosed;
		bannerAd.AdLeftApplication += handleBannerAdLeftApplication;
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
		if (!interstitialAd.IsLoaded ()) {
			//playerController.showDefaultPopUp ("Loading Ad.");
			interstitialAd.LoadAd (request);
		}
	}

	private void loadBannerAd(){
		if(!bannerAdLoaded)
			bannerAd.LoadAd (bannerAdRequest);
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
		//playerController.showErrorPopUp ("Error Failed to Load.");
	}
	
	public void showInterstitialAd(){
		if (interstitialAd.IsLoaded ())
			interstitialAd.Show ();
		else{
			//loadInterstitialAd();
			//interstitialAd.Show ();
		}
	}

	public void showBannerAd(){
		if (!bannerAdLoaded)
			bannerAd.LoadAd (bannerAdRequest);
		bannerAd.Show ();
	}

	public void hideBannerAd(){
		bannerAd.Hide ();
	}

	public void handleAdLoaded(object sender, EventArgs args){
		Debug.Log ("Ad Loaded.");
		//playerController.showDefaultPopUp ("Ad Loaded.");
	}

	public void handleAdOpened(object sender, EventArgs args){
		Debug.Log ("Ad Opened.");
	}

	void handleBannerAdLoaded(object sender, EventArgs args){
		bannerAdLoaded = true;
		Debug.Log ("Banner Ad Loaded!");
	}

	void handleBannerAdFailedToLoad(object sender, EventArgs args){
		Debug.Log ("Banner Ad Failed To Load!");
	}

	void handleBannerAdOpened(object sender, EventArgs args){
		Debug.Log ("Banner Ad Opened!");
	}

	void handleBannerAdClosing(object sender, EventArgs args){
		Debug.Log ("Banner Ad Closing!");
	}

	void handleBannerAdClosed(object sender, EventArgs args){
		Debug.Log ("Banner Ad Closed!");
	}

	void handleBannerAdLeftApplication(object sender, EventArgs args){
		Debug.Log ("Banner Ad Left Application!");
	}
}
