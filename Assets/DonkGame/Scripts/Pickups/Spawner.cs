using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {


	public float spawnDelay = 0f;
	public GameObject objectToSpawn = null;
	private GameObject spawnedObject = null;

	private bool spawned = false;
	private float timeOfDeath = 0f;
	private int numTimesSpawned = 0;
    private Damagable damagable = null;

	private bool CanSpawn {
		get {
			return ((Time.time - this.timeOfDeath) > this.spawnDelay) &&
        ((this.spawnedObject == null) || !(damagable.IsAlive));
		}
	}

    void Start() {
        this.SpawnObject();
    }

	void Update() {
		if(this.CanSpawn) {
            this.SpawnObject();
		}	
	}

    public void ObjectReadyForRespawn() {
        this.timeOfDeath = Time.time;
    }

	private void SpawnObject() {
        if (!this.spawned) {
            this.spawnedObject = (GameObject)GameObject.Instantiate(objectToSpawn, this.transform.position, this.objectToSpawn.transform.rotation);
            this.spawned = true;
            this.numTimesSpawned += 1;

            this.damagable = spawnedObject.GetComponent<Damagable>();
            if(this.damagable != null) {
                this.damagable.OnDie.AddListener(this.ObjectReadyForRespawn);
                this.damagable.Reset();
            }

        } else {
            this.spawnedObject.transform.position = this.transform.position;
            this.spawnedObject.SetActive(true);
            ++this.numTimesSpawned;

            if (this.damagable != null) {
                this.damagable.Reset();
            }
        }
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(this.transform.position, 0.2f);
	}
}
