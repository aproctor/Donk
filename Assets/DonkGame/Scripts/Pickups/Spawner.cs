using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public float spawnDelay = 0f;
	public GameObject objectToSpawn = null;
	private GameObject spawnedObject = null;

	public bool respawn = true;
	public float respawnDelay = 0f;

	private bool spawned = false;
	private float lastSpawnTime = 0f;
	private int numTimesSpawned = 0;

	private bool CanSpawn {
		get {
			//TODO implement spawn delay based on game round time
			return (spawnedObject == null);
		}
	}

	void Update() {
		if(this.CanSpawn) {
			SpawnObject();
		}	
	}

	private void SpawnObject() {
		spawnedObject = (GameObject)GameObject.Instantiate(objectToSpawn);
    this.lastSpawnTime = Time.time;
    spawned = true;
    numTimesSpawned += 1;
	}
}
