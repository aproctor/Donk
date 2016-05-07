using UnityEngine;
using System.Collections;

public class StoreModelBob : MonoBehaviour {

	public float bobHeight = 0.5f;
	public float rotationSpeed = 1f;

	public Vector3 originalPosition;
	public Quaternion originalRotation;

	void Start() {
		this.originalPosition = this.transform.position;
		this.originalRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = this.originalPosition + new Vector3 (0f, Mathf.Sin(Time.time), 0f) * bobHeight;
		this.transform.Rotate(new Vector3 (0f, rotationSpeed, 0f));
	}
}
