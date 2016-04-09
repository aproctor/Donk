using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Game.Initialize();

        //Additivly load the environment if not found
        //Not sure if I like this, but just starting to play with multi-scene editing
        if (GameObject.Find("Game-Environment") == null) {
            Debug.Log("Loading Environment");
            SceneManager.LoadScene((int)Game.Scenes.Environment, LoadSceneMode.Additive);
        }
	}
	
}
