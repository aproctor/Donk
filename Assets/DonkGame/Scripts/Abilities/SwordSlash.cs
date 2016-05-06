using UnityEngine;
using System.Collections;
using System;

public class SwordSlash : Ability {

	public int damageAmount = 30;
	[SerializeField]
    private float radius = 2;

	public SwordSlash(Transform playerTransform, LayerMask layer, Player player) : base(playerTransform, layer, player) {
		
	}



    public override void Activate() {
        Collider[] hitColliders = Physics.OverlapSphere(this.playerTransform.position, this.radius, this.mask.value);
        for(int i = 0; i < hitColliders.Length; ++i) {
            Damagable hitDamagable = hitColliders[i].GetComponent<Damagable>();
            if(hitDamagable != null) {
				ApplyDamage(hitDamagable, damageAmount);
            }
        }
    }
}
