using UnityEngine;
using System.Collections;
using System;

public class Crossbow : Ability {

  [SerializeField]
  GameObject bolt;
  [SerializeField] int damageAmount = 10;

  public override void Activate() {
    GameObject boltGO = (GameObject)Instantiate(this.bolt, this.transform.position + (this.transform.forward * 2) + (this.transform.up * 2), this.transform.rotation);
    boltGO.GetComponentInChildren<Bolt>().Shoot(this);
  }

  public void HitTarget(Damagable target) {
    Player hitPlayer = target.gameObject.GetComponent<Player>();
    if(hitPlayer == null || (hitPlayer.team != this.player.team)) {
      this.ApplyDamage(target, this.damageAmount);
    }
  } 
}
