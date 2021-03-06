﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class Game {

    public static GameRound round = null;

    public enum Scenes {
        //Enum values corespond to build load order
        Startup=0,
        Game=1,
        ForestEnvironment = 2
    }

    public enum CameraMode {
        Single,
        SideBySide, //2 screens split vertically
        TwoStacked, //2 screens split horizontally 
        TwoByTwo
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

    public static int NumPlayers {
        get {
            //TODO move this to game round when it's ready
            return Config.maxPlayers;
        }
    }

    public static void StartGame(GameMode mode) {
        round = new GameRound(mode);
        LoadLevel(Game.Scenes.Game);
    }

    public static void LoadLevel(Scenes scene) {
        SceneManager.LoadScene((int)scene);        
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
