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
            Player hitPlayer = hitColliders[i].GetComponent<Player>();
            if(hitPlayer != null) {
                Debug.LogError("hit a boy " + hitPlayer.playerNumber);
                hitPlayer.TakeDamage(30);
            }
        }
    }
}
