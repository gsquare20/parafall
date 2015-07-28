using UnityEngine;
using System.Collections;

public class DestroyParachute : MonoBehaviour {

	private ParafallObjectPool parafallObjectPool;

	private GameData gameData;

	// Use this for initialization
	void Start () {
		parafallObjectPool = ParafallObjectPool.Instance;
		gameData = GameData.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		//Destroy(collision.gameObject);
		parafallObjectPool.putObjectBackToPool(collision.gameObject);

		//Decrease player Health to 1
		gameData.setPlayerHealth (gameData.getPlayerHealth () -1f);
	}
}
