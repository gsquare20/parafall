using UnityEngine;
using System.Collections;

public class PacketVisibleByCamera : MonoBehaviour {

	private bool isPacketVisibleByCamera = false;

	void OnBecameVisible(){
		//isPacketVisibleByCamera = true;
		//Debug.Log ("On became visible called.");
	}

	void OnBecameInvisible(){
		//isPacketVisibleByCamera = false;
		//Debug.Log ("On became invisible called.");
	}

	public bool isPacketVisible()
	{
		return isPacketVisibleByCamera;
	}

	void Update(){
		Debug.Log ("Is packet visible : " + isPacketVisibleByCamera.ToString ());
		Vector3 viewPos = Camera.main.WorldToViewportPoint (this.transform.position);
		if(viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
			isPacketVisibleByCamera = true;
	}
}
