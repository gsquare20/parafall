using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParaPacket : System.Object{
	
	public GameObject paraGameObject;
	public string paraName;
	public int fallFrequency;
	public int initialPoolSize;
	public int maxPoolSize;
	public bool growToFillObjectPool;
}


public class ParafallObjectPool : MonoBehaviour {

	private static Dictionary<string, List<GameObject>> dictOfObjectsInPool = new Dictionary<string, List<GameObject>>();

	public List<ParaPacket> listOfPackets;

	private int objectsSpawnedFromPool = 1;

	private static ParafallObjectPool instance;

	public delegate void SpawnPowerUpAction (string paraPacketName);
	public static event SpawnPowerUpAction spawnPowerUpEvent;

	public static ParafallObjectPool Instance {
		get {
			if(null == instance){
				instance = GameObject.Find ("GameManager").GetComponent<ParafallObjectPool>();
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

	void Start(){
		initializeObjectPool ();
	}

	private void initializeObjectPool(){
		foreach (ParaPacket paraPacket in listOfPackets) {
			int paraPacketInitialPoolSize = paraPacket.initialPoolSize;
			for(int objectCounter=0; objectCounter<paraPacketInitialPoolSize; objectCounter++){
				GameObject tempGO = (GameObject)Instantiate (paraPacket.paraGameObject);
				tempGO.SetActive(false);
				if(dictOfObjectsInPool.ContainsKey(paraPacket.paraName)){
					dictOfObjectsInPool[paraPacket.paraName].Add(tempGO);
				}else{
					List<GameObject> tempGOList = new List<GameObject>();
					tempGOList.Add(tempGO);
					dictOfObjectsInPool.Add (paraPacket.paraName, tempGOList);
				}
				
			}
		}

		Debug.Log ("Object Pool initialized");
	}

	public void resetObjectPool(){
		foreach (string key in dictOfObjectsInPool.Keys) {
			List<GameObject> goList = dictOfObjectsInPool[key];
			foreach(GameObject gameObject in goList){
				gameObject.SetActive(false);
			}
		}
	}

	public List<GameObject> getObjectsOfType(string objectType){
		if(dictOfObjectsInPool.ContainsKey(objectType))
			return dictOfObjectsInPool[objectType];
		else
			return null;
	}

	public GameObject getObjectFromPool(){

		GameObject goToReturn = null;

		string objectTypeToSpawn = "foodpacket";

		//Debug.Log ("Object # spawned from pool : " + objectsSpawnedFromPool);

		foreach (string key in dictOfObjectsInPool.Keys) {
			ParaPacket paraPacket = getParaPacketDetails (key);
			int paraFrequency = paraPacket.fallFrequency;
			if(paraFrequency > 0 && objectsSpawnedFromPool%paraFrequency == 0){
				//Debug.Log ("Remainder : " + objectsSpawnedFromPool%paraFrequency);
				string paraPacketName = paraPacket.paraName;
				if(paraPacketName.Contains ("poweruppacket"))
					spawnPowerUpEvent(paraPacketName);
				else
					objectTypeToSpawn = paraPacket.paraName;
			}
		}

		if(dictOfObjectsInPool.ContainsKey(objectTypeToSpawn)){
			List<GameObject> tempGOs = dictOfObjectsInPool[objectTypeToSpawn];
			foreach(GameObject tempGO in tempGOs){
				if(tempGO.activeSelf == false){
					goToReturn = tempGO;
					break;
				}
			}
		}

		ParaPacket paraPacketToSpawn = getParaPacketDetails (objectTypeToSpawn);

		if(null == goToReturn && paraPacketToSpawn.growToFillObjectPool && dictOfObjectsInPool[paraPacketToSpawn.paraName].Count < paraPacketToSpawn.maxPoolSize)
		{
			GameObject tempGO = (GameObject)Instantiate (paraPacketToSpawn.paraGameObject);
			tempGO.SetActive(false);
			if(dictOfObjectsInPool.ContainsKey(paraPacketToSpawn.paraName)){
				dictOfObjectsInPool[paraPacketToSpawn.paraName].Add(tempGO);
			}else{
				List<GameObject> tempGOList = new List<GameObject>();
				tempGOList.Add(tempGO);
				dictOfObjectsInPool.Add (paraPacketToSpawn.paraName, tempGOList);
			}

			goToReturn = tempGO;
		}

		if(null != goToReturn)
			goToReturn.SetActive(true);

		objectsSpawnedFromPool++;

		return goToReturn;
	}

	public GameObject getObjectFromPool(string objectType){
		
		GameObject goToReturn = null;
		
		string objectTypeToSpawn = objectType;

		if(dictOfObjectsInPool.ContainsKey(objectTypeToSpawn)){
			List<GameObject> tempGOs = dictOfObjectsInPool[objectTypeToSpawn];
			foreach(GameObject tempGO in tempGOs){
				if(tempGO.activeSelf == false){
					goToReturn = tempGO;
					break;
				}
			}
		}
		
		ParaPacket paraPacketToSpawn = getParaPacketDetails (objectTypeToSpawn);
		
		if(null == goToReturn && paraPacketToSpawn.growToFillObjectPool && dictOfObjectsInPool[paraPacketToSpawn.paraName].Count < paraPacketToSpawn.maxPoolSize)
		{
			GameObject tempGO = (GameObject)Instantiate (paraPacketToSpawn.paraGameObject);
			tempGO.SetActive(false);
			if(dictOfObjectsInPool.ContainsKey(paraPacketToSpawn.paraName)){
				dictOfObjectsInPool[paraPacketToSpawn.paraName].Add(tempGO);
			}else{
				List<GameObject> tempGOList = new List<GameObject>();
				tempGOList.Add(tempGO);
				dictOfObjectsInPool.Add (paraPacketToSpawn.paraName, tempGOList);
			}
			
			goToReturn = tempGO;
		}
		
		if(null != goToReturn)
			goToReturn.SetActive(true);
		
		return goToReturn;
	}

	public void putObjectBackToPool(GameObject go){
		go.SetActive(false);
	}

	private ParaPacket getParaPacketDetails(string paraName){
		foreach (ParaPacket paraPacket in listOfPackets) {
			if(paraName.Equals(paraPacket.paraName))
				return paraPacket;
		}

		return null;
	}
}
