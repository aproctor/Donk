using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {
  public float weight = 10;

  private Rigidbody myRigidbody;

  void Start() {
    this.myRigidbody = this.GetComponent<Rigidbody>();
  }
  public IEnumerator SpatOut() {
		Collider c = this.GetComponent<Collider>();

    this.gameObject.SetActive(true);
    this.transform.parent = null;
    this.myRigidbody.isKinematic = false;
    c.enabled = false;
    this.myRigidbody.AddForce(Vector3.up * 500f);

    yield return new WaitForSeconds(1);
    c.enabled = true;
    yield return new WaitForSeconds(5);
    this.myRigidbody.isKinematic = true;

    yield return null;
  }

}
