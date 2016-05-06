using UnityEngine;
using System.Collections;

public class LevelConfig : MonoBehaviour {

  public Transform[] Team1SpawnPoints;
  public Transform[] Team2SpawnPoints;

	// Use this for initialization
	void Start () {
        GameMaster gm = GameObject.FindObjectOfType<GameMaster>();
        gm.LevelLoaded(this);
	}


}
