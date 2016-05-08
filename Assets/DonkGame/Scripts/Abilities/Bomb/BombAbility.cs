using UnityEngine;
using System.Collections;
using System;

public class BombAbility : Ability {

	public int damageAmount = 30;

	public GameObject projectilePrefab;

  public override void Activate() {
    if (!this.OnCooldown()) {
      this.lastUseTime = Time.time;

    if (this.type == Ability.Type.CONSUMABLE) {
      --this.chargesRemaining;
    }

    GameObject spawnedProjectile = (GameObject)GameObject.Instantiate (projectilePrefab);
      Bomb bomb = spawnedProjectile.GetComponent<Bomb>();
      if(bomb != null) {
        bomb.Toss(this);
      }

		  spawnedProjectile.transform.position = this.transform.position + this.transform.forward * 2;
    }
  }
}
