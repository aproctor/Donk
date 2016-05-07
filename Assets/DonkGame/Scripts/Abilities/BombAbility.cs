using UnityEngine;
using System.Collections;
using System;

public class BombAbility : Ability {

	public GameObject projectilePrefab;

    public override void Activate() {
		GameObject spawnedProjectile = (GameObject)GameObject.Instantiate (projectilePrefab);
		spawnedProjectile.transform.position = this.transform.position + this.transform.forward * 2;
    }
}
