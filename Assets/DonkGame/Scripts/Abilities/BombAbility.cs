using UnityEngine;
using System.Collections;
using System;

public class BombAbility : Ability {

	public int damageAmount = 30;
	[SerializeField]
  	private float radius = 2;

	public GameObject projectilePrefab;

    public override void Activate() {
		GameObject spawnedProjectile = (GameObject)GameObject.Instantiate (projectilePrefab);
		spawnedProjectile.transform.position = this.transform.position + this.transform.forward * 2;
    }
}
