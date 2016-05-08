using UnityEngine;
using System.Collections;

public class LandMine : MonoBehaviour {

  [SerializeField]
  float damage;
  [SerializeField]
  GameObject explosionSystem;

  void OnTriggerEnter(Collider collider) {
    Damagable damagable = collider.gameObject.GetComponent<Damagable>();
    if(damagable != null) {
      damagable.TakeDamage(this.damage);
    }

    this.explosionSystem.SetActive(true);

    Destroy(this.gameObject, 2f);

  }
}
