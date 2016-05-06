using UnityEngine;
using System.Collections;

public class BigEgg : Damagable {

  void OnTriggerEnter(Collider other) {
    ChickenCoop coop = other.GetComponent<ChickenCoop>();
    //TODO figure out how to score the big egg
    if(coop != null) {
      this.Die(coop.team);
    }
  }
}
