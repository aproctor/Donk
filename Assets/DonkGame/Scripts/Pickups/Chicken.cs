using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {
  public float weight = 10;

  [SerializeField]
  SphereCollider collider;

  private Rigidbody myRigidbody;

  void Start() {
    this.myRigidbody = this.GetComponent<Rigidbody>();
  }
  public IEnumerator SpatOut() {
    this.gameObject.SetActive(true);
    this.transform.parent = null;
    this.myRigidbody.isKinematic = false;
    this.collider.enabled = false;
    this.myRigidbody.AddForce(Vector3.up * 500f);

    yield return new WaitForSeconds(1);
    this.collider.enabled = true;
    yield return new WaitForSeconds(5);
    this.myRigidbody.isKinematic = true;

    yield return null;
  }

}
