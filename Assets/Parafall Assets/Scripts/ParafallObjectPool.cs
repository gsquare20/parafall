using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParafallObjectPool : MonoBehaviour {

	//private static KeyValuePair<string, List<string>> kvp = new KeyValuePair<string, List<string>>();
	private static Dictionary<string, List<GameObject>> dictOfObjectsInPool = new Dictionary<string, List<GameObject>>();

	public int initialPoolSize = 0;

	public GameObject targetObjectToPreInstantiate;

	public string objectTypeToPreInstantiate = "parachute";

	public int maxPoolSize = 15;

	public bool growToFillObjectPool = false;

	void Start(){
		if(initialPoolSize > 0){
			if(null != targetObjectToPreInstantiate){
				for(int objectCounter=0; objectCounter<initialPoolSize; objectCounter++){
					GameObject tempGO = (GameObject)Instantiate (targetObjectToPreInstantiate);
					tempGO.SetActive(false);
					if(dictOfObjectsInPool.ContainsKey(objectTypeToPreInstantiate)){
						dictOfObjectsInPool[objectTypeToPreInstantiate].Add(tempGO);
					}else{
						List<GameObject> tempGOList = new List<GameObject>();
						tempGOList.Add(tempGO);
						dictOfObjectsInPool.Add (objectTypeToPreInstantiate, tempGOList);
					}
					 
				}
			}
		}
	}

	public List<GameObject> getObjectsOfType(string objectType){
		if(dictOfObjectsInPool.ContainsKey(objectType))
			return dictOfObjectsInPool[objectType];
		else
			return null;
	}

	public GameObject getObjectFromPool(string objectType){
		GameObject goToReturn = null;
		if(dictOfObjectsInPool.ContainsKey(objectType)){
			List<GameObject> tempGOs = dictOfObjectsInPool[objectType];
			foreach(GameObject tempGO in tempGOs){
				if(tempGO.activeSelf == false){
					goToReturn = tempGO;
					break;
				}
			}
		}
		return goToReturn;
	}

	public void putObjectBackToPool(GameObject go){
		go.SetActive(false);
	}
}
