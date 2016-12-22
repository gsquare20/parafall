using UnityEngine;
using System.Collections;

public class GrabParachute : MonoBehaviour {

	private ParafallObjectPool parafallObjectPool;

	private GameData gameData;

	// Use this for initialization
	void Start () {
		parafallObjectPool = ParafallObjectPool.Instance;
		gameData = GameData.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("");
	}

	void OnCollisionEnter2D(Collision2D collision) {
		//Destroy(collision.gameObject);
		//Transform collisionGOTransform = collision.gameObject.transform;
		parafallObjectPool.putObjectBackToPool(collision.gameObject);
		gameData.setPlayerScore (gameData.getPlayerScore () + 1);
	}

	void OnCollisionExit2D(Collision2D collision) {
		//Destroy(collision.gameObject);
		//Transform collisionGOTransform = collision.gameObject.transform;
		parafallObjectPool.putObjectBackToPool(collision.gameObject);
		gameData.setPlayerScore (gameData.getPlayerScore () + 1);
	}
}
