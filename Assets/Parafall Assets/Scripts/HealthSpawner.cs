using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthSpawner : MonoBehaviour {

	public List<float> listOfWhenHeathPacketShouldBeSpawned;

	public float fallSpeed = 150f;

	private Transform myTransform;

	private ParafallObjectPool parafallObjectPool;

	public string objectTypeToSpawn = "healthpacket";

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
		GameData.playerHealthChangeEvent += spawnHealthPacket;
	}

	void OnDisable() {
		GameData.playerHealthChangeEvent -= spawnHealthPacket;
	}

	void spawnHealthPacket(float playerHealth){
		foreach (float healthVal in listOfWhenHeathPacketShouldBeSpawned) {
			//Debug.Log ("Health Val : " + healthVal);
			//Debug.Log ("Player Health : " + playerHealth);
			if(healthVal == playerHealth){
				GameObject tempGO = parafallObjectPool.getObjectFromPool(objectTypeToSpawn);
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
		}
	}

}
