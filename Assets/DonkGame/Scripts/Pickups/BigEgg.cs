using UnityEngine;
using System.Collections;

public class BigEgg : Damagable
{

	void OnTriggerEnter (Collider other)
	{
		ChickenCoop coop = other.GetComponent<ChickenCoop> ();
		if (coop != null) {			
			coop.team.AddPoints (Game.round.mode.eggCaptureScoreAmount);
			coop.team.AddGold (Game.round.mode.eggCaptureGoldAmount);
			this.Die ();
		}
	}
}
