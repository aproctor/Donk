using UnityEngine;
using System.Collections;
using System;

public class Crossbow : Ability {

  [SerializeField]
  GameObject bolt;
  [SerializeField] int damageAmount = 10;

  public override void Activate() {
    if (!this.OnCooldown()) {
      this.lastUseTime = Time.time;

      if (this.type == Ability.Type.CONSUMABLE) {
        --this.chargesRemaining;
      }

      GameObject boltGO = (GameObject)Instantiate(this.bolt, this.transform.position + (this.transform.forward * 2) + (this.transform.up * 2), this.transform.rotation);
      boltGO.GetComponentInChildren<Bolt>().Shoot(this);
    }
  }

  public void HitTarget(Damagable target) {
	BigEgg egg = target.GetComponent<BigEgg> ();
    Player hitPlayer = target.gameObject.GetComponent<Player>();
    //This logic could be drastically simplified just by checking the layer mask, but that's not working and this is a jam so fuck it
    if(egg == null && (hitPlayer == null || (hitPlayer.team != this.player.team))) {
      this.ApplyDamage(target, this.damageAmount);
    }
  } 
}
