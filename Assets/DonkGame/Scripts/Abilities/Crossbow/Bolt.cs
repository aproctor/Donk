using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

  [SerializeField]
  float flightSpeed = 10;

  private Crossbow crossbow;

  void FixedUpdate() {
    this.gameObject.GetComponent<Rigidbody>().MovePosition(this.transform.position + (this.transform.forward * flightSpeed));
  }

  public void Shoot(Crossbow crossbow) {
    this.crossbow = crossbow;
    Destroy(this.gameObject, 2f);
  }

  void OnTriggerEnter(Collider collider) {
    Damagable damagable = collider.gameObject.GetComponent<Damagable>();
    if(damagable != null) {
      this.crossbow.HitTarget(damagable);
    }
    Destroy(this.gameObject);
  }

}
