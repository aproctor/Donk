using UnityEngine;
using System.Collections;

public class ChickenCoop : MonoBehaviour {

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
}
