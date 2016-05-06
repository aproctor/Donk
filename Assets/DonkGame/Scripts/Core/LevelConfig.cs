using UnityEngine;
using System.Collections;

public class LevelConfig : MonoBehaviour {

  public TeamObjects[] teamObjects;

	// Use this for initialization
	void Start () {
        GameMaster gm = GameObject.FindObjectOfType<GameMaster>();
        gm.LevelLoaded(this);
	}


}
