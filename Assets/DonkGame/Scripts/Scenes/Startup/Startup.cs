using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Game.Initialize();
	}

    void Update() {
        if (Input.anyKeyDown) {
            //TODO level select in start menus
            Game.StartGame(Game.Config.modes[0]);
        }
    }
}
