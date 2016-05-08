using UnityEngine;
using System.Collections;
using System;

public class SwordSlash : Ability {

	public int damageAmount = 30;
	[SerializeField]
  private float radius = 2;

	[SerializeField]
	private GameObject slashFX;


  public override void Activate() {
    if (!this.OnCooldown()) {
			this.slashFX.SetActive(false);
			this.slashFX.SetActive(true);
      this.lastUseTime = Time.time;

      if(this.type == Ability.Type.CONSUMABLE) {
        --this.chargesRemaining;
      }

      Collider[] hitColliders = Physics.OverlapSphere(this.playerTransform.position, this.radius, this.mask.value);

      for (int i = 0; i < hitColliders.Length; ++i) {
        Damagable hitDamagable = hitColliders[i].GetComponent<Damagable>();
        if (hitDamagable != null) {
          ApplyDamage(hitDamagable, damageAmount);
        }
      }
    }
  }

}
