using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour {

	public float fallSpeed = 150f;

	private Transform myTransform;

	public GameObject powerUpSpawner1GO;

	public GameObject powerUpSpawner2GO;

	private ParafallObjectPool parafallObjectPool;

	private PowerUpManager powerUpManager;

	private Transform spawnedGO;

	// Use this for initialization
	void Start () {
		myTransform = this.transform;
		parafallObjectPool = ParafallObjectPool.Instance;
		powerUpManager = PowerUpManager.Instance;
		//InvokeRepeating("spawnParachute", firstInvokeTime, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {
//		if(null != spawnedGO)
//			maintainDistanceWithOtherParaPacketsOfSameLane (spawnedGO);
	}

	void OnEnable(){
		ParafallObjectPool.spawnPowerUpEvent += spawnPowerUpPacket;
	}

	void OnDisable() {
		ParafallObjectPool.spawnPowerUpEvent -= spawnPowerUpPacket;
	}

	void spawnPowerUpPacket(string paraPacketName){
		int spawnerNo = (int)Random.Range (0F, 10F);
		Debug.Log ("Spawner Number : " + spawnerNo);
		Transform spawnerTransformGO;
		if (spawnerNo < 5)
			spawnerTransformGO = powerUpSpawner1GO.transform;
		else
			spawnerTransformGO = powerUpSpawner2GO.transform;

		GameObject tempGO = parafallObjectPool.getObjectFromPool(paraPacketName);
		if (null != tempGO) {
			spawnedGO = tempGO.transform;
			tempGO.transform.position = spawnerTransformGO.position;
			tempGO.transform.rotation = spawnerTransformGO.rotation;
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

	void maintainDistanceWithOtherParaPacketsOfSameLane(Transform transform){
		RaycastHit2D hit = Physics2D.Raycast (new Vector2(transform.position.x, transform.position.y-2.2F), -Vector2.up);
		if (null != hit.collider) {
			string hitGOName = hit.transform.gameObject.name;
			//Debug.Log ("hit game object name : " + hit.transform.gameObject.name);
			//Debug.Log ("transform game object name : " + transform.gameObject.name);
			Debug.DrawRay(new Vector3(transform.position.x, transform.position.y-2.2F, transform.position.z), Vector3.down);
			//hit.transform.Rotate(new Vector2(hit.transform.position.x+70, hit.transform.position.y));
			float distance = Mathf.Abs(hit.point.y - transform.position.y);
			//Debug.Log ("Distance between object and its hit is : " + distance);
			if((hitGOName.Contains("powerup") || hitGOName.Contains("health")) && distance < 5){
				transform.Translate(new Vector3(transform.position.x, transform.position.y + 4));
			}
		}



		//List<GameObject> listOfHealthBarPackets = parafallObjectPool.getObjectsOfType ("healthpacket");
		//List

	}

}
