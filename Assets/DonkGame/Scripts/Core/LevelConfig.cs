using UnityEngine;
using System.Collections;

public class LevelConfig : MonoBehaviour {

    //TODO put spawn point and other geographically important data here

	// Use this for initialization
	void Start () {
        GameMaster gm = GameObject.FindObjectOfType<GameMaster>();
        gm.LevelLoaded(this);
	}


}
