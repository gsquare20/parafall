using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BoostUpObject{
	public string id;
	public string title;
	public int coinValue;
	public string desc;
}

#if UNITY_EDITOR
public class BoostUpStoreCreator {
	[MenuItem("Window/Parafall Boost Up Store Creator")]
	public static void createBoostUpStore(){
		BoostUpStore boostUpStore = ScriptableObject.CreateInstance<BoostUpStore> ();
		AssetDatabase.CreateAsset (boostUpStore, "Assets/Parafall Assets/Stores/BoostUpStore.asset");
		AssetDatabase.SaveAssets ();
		
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = boostUpStore;
	}
}
#endif

[System.Serializable]
public class BoostUpStore : ScriptableObject {

	public List<BoostUpObject> boostUpObjList = new List<BoostUpObject>();

}
