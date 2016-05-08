using UnityEngine;
using System.Collections;

public class LandMine : MonoBehaviour {

  [SerializeField]
  float damage;
  [SerializeField]
  GameObject explosion;
  [SerializeField]
  GameObject model;

  void OnTriggerEnter(Collider collider) {
    Damagable damagable = collider.gameObject.GetComponent<Damagable>();
    if(damagable != null) {
      this.model.SetActive(false);
      damagable.TakeDamage(this.damage);
      Instantiate(this.explosion, this.transform.position, this.explosion.transform.rotation);
      Destroy(this.gameObject, 2f);
    }


  }
}
