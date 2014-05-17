using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParachuteSpawner : MonoBehaviour {

	public float spawnRate = .5f;

	public float firstInvokeTime = .5f;

	public GameObject parachuteGO;

	private Transform myTransform;

	// Use this for initialization
	void Start () {
		myTransform = this.transform;
		InvokeRepeating("spawnParachute", firstInvokeTime, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void spawnParachute() {
		GameObject tempGO = (GameObject)Instantiate (parachuteGO, myTransform.position, myTransform.rotation);

		GUIText childGuiTextObj = tempGO.transform.GetChild (0).guiText;
		float randomNum = Random.Range (0F, 1000F);
		int randomInt = (int)randomNum;
		//Debug.Log ("Generated random number : " + randomInt.ToString());
		childGuiTextObj.text = randomInt.ToString ();
		/*Vector3 tempGONormalizedPosition = Camera.mainCamera.WorldToViewportPoint(tempGO.transform.position);
		Debug.Log("tempGO nomalized position y : "  + tempGONormalizedPosition.y);
		tempGONormalizedPosition.y = tempGONormalizedPosition.y + 10F;
		childGuiTextObj.transform.position =  tempGONormalizedPosition;*/

	}

}
