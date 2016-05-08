using UnityEngine;
using System.Collections;
using System;

public class LandMineAbility : Ability {

  [SerializeField]
  GameObject landMinePrefab;

  public override void Activate() {
    if (!this.OnCooldown()) {
      this.lastUseTime = Time.time;

      GameObject spawnedProjectile = (GameObject)GameObject.Instantiate(landMinePrefab, (this.transform.position + (-this.transform.forward * 2)), landMinePrefab.transform.rotation);
      LandMine landMine = spawnedProjectile.GetComponent<LandMine>();

      //hard coded garbage
      switch (this.player.team.teamNumber) {
        case 1:
          landMine.gameObject.layer = 11;
          break;
        case 2:
          landMine.gameObject.layer = 12;
          break;
        default:
          break;
      }
    }
  }

}
