using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Game.Initialize();
	}

    void Update() {
        if (Input.anyKeyDown) {
            Game.LoadLevel(Game.Scenes.Game);
        }
    }
}
