using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParachuteSpawner : MonoBehaviour {

	public float spawnRate = .5f;

	public float firstInvokeTime = .5f;

	public float fallSpeed = 100f;

	private Transform myTransform;

	private ParafallObjectPool parafallObjectPool;

	private float powerUpToken = 1f;
		
	public GameObject goToGrabAll;

	// Use this for initialization
	void Start () {
		myTransform = this.transform;
		parafallObjectPool = ParafallObjectPool.Instance;
		//InvokeRepeating("spawnParachute", firstInvokeTime, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnEnable(){
		ParafallEventManager.playerScoreAndParachuteSpeedEvent += setParachuteFallSpeed;
		StateManager.startSpawningParaPacketsEvent += startSpawning;
		StateManager.initStateEvent += stopCoroutineToSpawnParaPackets;
		PowerUpManager.powerUpInUseEvent += powerUpInUse;
		PowerUpManager.powerUpNotInUseEvent += powerUpNotInUse;
	}

	void OnDisable() {
		ParafallEventManager.playerScoreAndParachuteSpeedEvent -= setParachuteFallSpeed;
		StateManager.startSpawningParaPacketsEvent -= startSpawning;
		StateManager.initStateEvent -= stopCoroutineToSpawnParaPackets;
		PowerUpManager.powerUpInUseEvent -= powerUpInUse;
		PowerUpManager.powerUpNotInUseEvent -= powerUpNotInUse;
	}

	void startSpawning(){
//		foreach (ParaPacket paraPacket in parafallObjectPool.listOfPackets) {
			//StartCoroutine (initialWaitBeforeSpawningParaPackets());
			StartCoroutine("startSpawningParaPackets");		
//		}
		//InvokeRepeating("spawnParachute", firstInvokeTime, spawnRate);
	}

	void spawnParachute() {
		//GameObject tempGO = (GameObject)Instantiate (parachuteGO, myTransform.position, myTransform.rotation);

		//TODO : Need to call ParafallObjectPool's static method to spawn a new parachute game object or to make it active
		GameObject tempGO = parafallObjectPool.getObjectFromPool();
		if (null != tempGO) {
			tempGO.transform.position = myTransform.position;
			tempGO.transform.rotation = myTransform.rotation;
			GUIText childGuiTextObj = tempGO.transform.GetChild (0).guiText;
			float randomNum = Random.Range (0F, 1000F);
			int randomInt = (int)randomNum;
			//Debug.Log ("Generated random number : " + randomInt.ToString());
			childGuiTextObj.text = randomInt.ToString ();
			tempGO.transform.rigidbody2D.velocity = new Vector2(0f, -(fallSpeed/powerUpToken));
			//tempGO.transform.rigidbody2D.AddForce (-Vector2.up * (fallSpeed/powerUpToken), ForceMode2D.Force);
			//StartCoroutine("waitFor5Seconds", tempGO.transform.rigidbody2D);

			/*Vector3 tempGONormalizedPosition = Camera.mainCamera.WorldToViewportPoint(tempGO.transform.position);
			Debug.Log("tempGO nomalized position y : "  + tempGONormalizedPosition.y);
			tempGONormalizedPosition.y = tempGONormalizedPosition.y + 10F;
			childGuiTextObj.transform.position =  tempGONormalizedPosition;*/
		}

	}

	IEnumerator startSpawningParaPackets(){
		yield return new WaitForSeconds (firstInvokeTime);
		while (true) {
			spawnParachute();
			//Debug.Log ("Spawn Rate : " + spawnRate);
			yield return new WaitForSeconds(spawnRate);
		}
	}

	IEnumerator initialWaitBeforeSpawningParaPackets(){
		yield return new WaitForSeconds (firstInvokeTime);
	}

	IEnumerator waitFor5Seconds(Rigidbody2D parachuteRigidbody){
		yield return new WaitForSeconds (5f);
		powerUpToken = 2f;
		parachuteRigidbody.velocity = new Vector2 (0f, -(fallSpeed / powerUpToken));
		yield return new WaitForSeconds (5f);
		powerUpToken = 1f;
		parachuteRigidbody.velocity = new Vector2 (0f, -(fallSpeed / powerUpToken));
	}

	void setParachuteFallSpeed(float fallSpeed){
		//Debug.Log ("Fall Speed increased to 200");
		this.fallSpeed = fallSpeed;
	}

	void stopCoroutineToSpawnParaPackets(){
		StopCoroutine ("startSpawningParaPackets");
	}

	void powerUpInUse(string powerUpName){
		if (powerUpName.Equals ("slowdownfallpowerup")){
			powerUpToken = 2f;
			toggleParaPacketsSpeed();
		}

		if (powerUpName.Equals ("graballpowerup")){
			goToGrabAll.SetActive (true);
			powerUpToken = .5f;
			toggleParaPacketsSpeed();
		}
	}
	
	void powerUpNotInUse(){
		powerUpToken = 1f;
		goToGrabAll.SetActive (false);
		toggleParaPacketsSpeed ();
	}

	void toggleParaPacketsSpeed(){
		string[] packetsArr = new string[] {"foodpacket", "coinpacket"};
		foreach (string packet in packetsArr) {
			List<GameObject> packetsList = parafallObjectPool.getObjectsOfType(packet);
			foreach(GameObject packetGO in packetsList){
				if(packetGO.activeSelf)
					packetGO.rigidbody2D.velocity = new Vector2(0f, -(fallSpeed / powerUpToken));
			}
		}
	}

}
