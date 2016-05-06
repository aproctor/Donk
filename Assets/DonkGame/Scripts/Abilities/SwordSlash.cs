using UnityEngine;
using System.Collections;
using System;

public class SwordSlash : Ability {

    private float radius = 2;
    private Transform playerTransform;
    private LayerMask mask;

    public SwordSlash(Transform playerTransform, LayerMask layer) {
        this.playerTransform = playerTransform;
        this.mask = layer;
    }

    public override void Activate() {
        Collider[] hitColliders = Physics.OverlapSphere(this.playerTransform.position, this.radius, this.mask.value);
        for(int i = 0; i < hitColliders.Length; ++i) {
            Damagable hitDamagable = hitColliders[i].GetComponent<Damagable>();
            if(hitDamagable != null) {
                hitDamagable.TakeDamage(30);
            }
        }
    }
}
