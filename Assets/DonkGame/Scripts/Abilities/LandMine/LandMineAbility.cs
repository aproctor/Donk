using UnityEngine;
using System.Collections;
using System;

public class LandMineAbility : Ability {

  [SerializeField]
  GameObject landMinePrefab;

  public override void Activate() {
    GameObject spawnedProjectile = (GameObject)GameObject.Instantiate(landMinePrefab, (this.transform.position + (this.transform.forward * 2)), landMinePrefab.transform.rotation);
    LandMine landMine = spawnedProjectile.GetComponent<LandMine>();
  }

}
