using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LandMine : MonoBehaviour {

  [SerializeField]
  int damage;
  [SerializeField]
  GameObject explosion;
  [SerializeField]
  GameObject model;

  public UnityEvent OnActivate;

  private LandMineAbility landMineAbility;

  public void Init(LandMineAbility landMineAbility) {
    this.landMineAbility = landMineAbility;
  }

  void OnTriggerEnter(Collider collider) {
    Damagable damagable = collider.gameObject.GetComponent<Damagable>();
    if(damagable != null) {
      this.model.SetActive(false);
      this.landMineAbility.ApplyDamage(damagable, this.damage);

      this.OnActivate.Invoke();

      Instantiate(this.explosion, this.transform.position, this.explosion.transform.rotation);
      Destroy(this.gameObject, 2f);
    }      
  }
}
