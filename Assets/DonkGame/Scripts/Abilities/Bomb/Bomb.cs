using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {

  [SerializeField]
  float chargeTime = 2f;
  [SerializeField]
  float explodeTime = 1f;
  [SerializeField]
  GameObject explosionSystem;
  [SerializeField] float explosionRadius = 2f;

  private BombAbility bombAbility;

  public void Toss(BombAbility bombAbility) {
    StartCoroutine(this.SetCharge());
    this.bombAbility = bombAbility;
  }

  private IEnumerator SetCharge() {
    yield return new WaitForSeconds(this.chargeTime);

    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.bombAbility.mask.value);

    for (int i = 0; i < hitColliders.Length; ++i) {
      Damagable hitDamagable = hitColliders[i].GetComponent<Damagable>();
      if (hitDamagable != null) {
        this.bombAbility.ApplyDamage(hitDamagable, this.bombAbility.damageAmount);
      }
    }

    this.explosionSystem.SetActive(true);

    yield return new WaitForSeconds(this.explodeTime);

    Destroy(this.gameObject);
  }
}
