using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class Game {

    public enum Scenes {
        Startup,
        Game
    }

    private static GameConfig _config = null;
    public static GameConfig Config {
        get {
            return _config;
        }
    }

    private static bool _initialized = false;
    public static void Initialize() {
        if (_initialized == false) {
            LoadConfig();
            _initialized = true;
        }
    }

    public static void LoadLevel(Scenes scene) {
        SceneManager.LoadScene(scene.ToString());
    }

    private static void LoadConfig() {
        TextAsset environmentAsset = (TextAsset)Resources.Load("Config/environment", typeof(TextAsset));
        string environment = null;
        if (environmentAsset == null || string.IsNullOrEmpty(environmentAsset.text)) {
            environment = "default";
        } else {
            environment = environmentAsset.text.Trim();
        }
        _config = (GameConfig)Resources.Load("Config/" + environment);
    }
}
