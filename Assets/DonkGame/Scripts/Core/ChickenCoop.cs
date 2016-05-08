using UnityEngine;
using System.Collections;

public class ChickenCoop : MonoBehaviour {

  [HideInInspector]
  public Team team;

  public int CountChickens() {
    SphereCollider chickenCoopCollider = this.GetComponent<SphereCollider>();

    int chickens = 0;
    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position + chickenCoopCollider.center, chickenCoopCollider.radius);
    for (int i = 0; i < hitColliders.Length; i++) {
      Chicken c = hitColliders[i].GetComponent<Chicken>();
      if (c != null) {
        ++chickens;
      }
    }

    return chickens;
  }

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color (255f, 0f, 0f, 0.2f);
		Gizmos.DrawSphere (this.transform.position, this.GetComponent<SphereCollider> ().radius);
	}
}
