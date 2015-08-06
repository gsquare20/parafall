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
		Transform collisionGOTransform = collision.gameObject.transform;
		parafallObjectPool.putObjectBackToPool(collision.gameObject);

		float transformX = Mathf.Round (collisionGOTransform.position.x);
		//Debug.Log ("Collision GO transform x : " + transformX);
		if(transformX == 0f || transformX == 10f || transformX == -10f)
			//Decrease player Health to 1
			gameData.setPlayerHealth (gameData.getPlayerHealth () - 1f);
	}
}
