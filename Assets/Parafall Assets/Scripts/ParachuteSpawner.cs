using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParachuteSpawner : MonoBehaviour {

	public float spawnRate = .5f;

	public float firstInvokeTime = .5f;

	public float fallSpeed = 100f;

	private Transform myTransform;

	private ParafallObjectPool parafallObjectPool;

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
	}

	void OnDisable() {
		ParafallEventManager.playerScoreAndParachuteSpeedEvent -= setParachuteFallSpeed;
		StateManager.startSpawningParaPacketsEvent -= startSpawning;
		StateManager.initStateEvent -= stopCoroutineToSpawnParaPackets;
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
			tempGO.transform.rigidbody2D.AddForce (-Vector2.up * fallSpeed, ForceMode2D.Force);
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

	void setParachuteFallSpeed(float fallSpeed){
		//Debug.Log ("Fall Speed increased to 200");
		this.fallSpeed = fallSpeed;
	}

	void stopCoroutineToSpawnParaPackets(){
		StopCoroutine ("startSpawningParaPackets");
	}


}
