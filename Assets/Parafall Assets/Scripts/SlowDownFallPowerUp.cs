//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class SlowDownFallPowerUp : IPlayerPowerUp
{
	private PowerUpManager powerUpManager;
	
	public SlowDownFallPowerUp(PowerUpManager powerUpManager){
		this.powerUpManager = powerUpManager;	
	}

	public void executePowerUpRelatedTasks(GameObject powerUpSlider){
		Debug.Log ("Slow down fall power up used.");
	}
}


