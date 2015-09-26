using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BoostUpObject{
	public string title;
	public int coinValue;
	public string desc;
}

public class BoostUpStoreCreator {
	[MenuItem("Window/Parafall Boost Up Store Creator")]
	public static void createBoostUpStore(){
		BoostUpStore boostUpStore = ScriptableObject.CreateInstance<BoostUpStore> ();
		AssetDatabase.CreateAsset (boostUpStore, "Assets/Parafall Assets/Stores/BoostUpStore");
		AssetDatabase.SaveAssets ();
		
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = boostUpStore;
	}
}

public class BoostUpStore : ScriptableObject {

	public List<BoostUpObject> boostUpObjList = new List<BoostUpObject>();

	void OnEnable(){
	}
}
