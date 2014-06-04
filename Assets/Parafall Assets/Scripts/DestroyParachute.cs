using UnityEngine;
using System.Collections;

public class DestroyParachute : MonoBehaviour {

	private ParafallObjectPool parafallObjectPool;

	// Use this for initialization
	void Start () {
		parafallObjectPool = ParafallObjectPool.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		//Destroy(collision.gameObject);
		parafallObjectPool.putObjectBackToPool(collision.gameObject);
	}
}
