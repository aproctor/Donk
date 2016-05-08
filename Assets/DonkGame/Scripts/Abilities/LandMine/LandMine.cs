using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LandMine : MonoBehaviour {

  [SerializeField]
  float damage;
  [SerializeField]
  GameObject explosion;
  [SerializeField]
  GameObject model;

  public UnityEvent OnActivate;

  void OnTriggerEnter(Collider collider) {
    Damagable damagable = collider.gameObject.GetComponent<Damagable>();
    if(damagable != null) {
      this.model.SetActive(false);
      damagable.TakeDamage(this.damage);

      this.OnActivate.Invoke();

      Instantiate(this.explosion, this.transform.position, this.explosion.transform.rotation);
      Destroy(this.gameObject, 2f);
    }      
  }
}
