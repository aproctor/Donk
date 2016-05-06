using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {


	public float spawnDelay = 0f;
	public GameObject objectToSpawn = null;
	private GameObject spawnedObject = null;

	private bool spawned = false;
	private float lastSpawnTime = 0f;
	private int numTimesSpawned = 0;

	private bool CanSpawn {
		get {
			return ((Time.time - this.lastSpawnTime) > spawnDelay) &&
                ((spawnedObject == null) || !(spawnedObject.activeSelf));
		}
	}

	void Update() {
		if(this.CanSpawn) {
			SpawnObject();
		}	
	}

	private void SpawnObject() {
        if (spawnedObject == null) {
            spawnedObject = (GameObject)GameObject.Instantiate(objectToSpawn, this.transform.position, this.objectToSpawn.transform.rotation);
            this.lastSpawnTime = Time.time;
            spawned = true;
            numTimesSpawned += 1;
        } else {
            this.spawnedObject.transform.position = this.transform.position;
            this.spawnedObject.SetActive(true);
            this.lastSpawnTime = Time.time;
            ++this.numTimesSpawned;
        }
	}
}
