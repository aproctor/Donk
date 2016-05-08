using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Bomb : MonoBehaviour {

  [SerializeField]
  float chargeTime = 2f;
  [SerializeField]
  float explodeTime = 1f;
  [SerializeField]
  GameObject explosion;
  [SerializeField]
  float explosionRadius = 2f;
  [SerializeField]
  GameObject model;

  public UnityEvent OnActivate;

  private BombAbility bombAbility;

  public void Toss(BombAbility bombAbility) {
    StartCoroutine(this.SetCharge());
    this.bombAbility = bombAbility;
  }

  private IEnumerator SetCharge() {
    yield return new WaitForSeconds(this.chargeTime);

    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, this.explosionRadius);

    for (int i = 0; i < hitColliders.Length; ++i) {
      Damagable hitDamagable = hitColliders[i].GetComponent<Damagable>();
			if (hitDamagable != null && hitDamagable.GetComponent<BigEgg>() == null) {
        this.bombAbility.ApplyDamage(hitDamagable, this.bombAbility.damageAmount);
      }
    }

    OnActivate.Invoke();

    Instantiate(this.explosion, this.transform.position, this.explosion.transform.rotation);
    this.model.SetActive(false);

    Destroy(this.gameObject, this.explodeTime);
  }
}
