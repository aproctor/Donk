using UnityEngine;
using System.Collections;
using System;

public class Crossbow : Ability {

  [SerializeField]
  GameObject bolt;
  [SerializeField] int damageAmount = 10;
  public float numBolts = 5;
  public float cone = 45f;

  private WaitForSeconds shotDelay = new WaitForSeconds(0.02f);

  public override void Activate() {
    if (!this.OnCooldown()) {
      this.lastUseTime = Time.time;

      if (this.type == Ability.Type.CONSUMABLE) {
        --this.chargesRemaining;
      }

      StartCoroutine(BoltSpray());
    }
  }

  IEnumerator BoltSpray() {
    for(int i = 0; i < numBolts; i++) {
      GameObject boltGO = (GameObject)Instantiate(this.bolt, this.transform.position + (this.transform.up), this.transform.rotation);

      if(i > 1) {
        boltGO.transform.Rotate(new Vector3(0f, (UnityEngine.Random.value * cone * 2f) - cone, 0f));
      }
      boltGO.transform.position = boltGO.transform.position + boltGO.transform.forward * 3;

      boltGO.GetComponentInChildren<Bolt>().Shoot(this);

      yield return shotDelay;
    }
    
    yield return null;
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
