using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {
  public float weight = 10;

  private Rigidbody myRigidbody;
  private bool standUp;

  void Start() {
    this.myRigidbody = this.GetComponent<Rigidbody>();
  }

  void Update() {
//    if (this.standUp) {
//      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0f, this.transform.rotation.y, 0f), 0.2f);
//      this.standUp = !((Mathf.Approximately(0.2f, this.transform.rotation.x) && Mathf.Approximately(0.2f, this.transform.rotation.z)));
//    }
  }

  public IEnumerator SpatOut() {
		Collider c = this.GetComponent<Collider>();

    this.gameObject.SetActive(true);
    c.enabled = false;
    this.myRigidbody.AddForce((this.transform.parent.up *Random.Range(400f, 500f)) + (-this.transform.parent.forward * Random.Range(100f, 150f)) + (this.transform.parent.right * Random.Range(-200f, 200f)));
    this.transform.parent = null;

    yield return new WaitForSeconds(1);
    c.enabled = true;

    yield return null;
  }

  void OnCollisionEnter(Collision collision) {
    if(collision.gameObject.name == "Ground") {
      this.standUp = true;
    }
  }

}
