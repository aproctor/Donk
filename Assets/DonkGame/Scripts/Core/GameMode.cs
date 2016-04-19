using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameMode : ScriptableObject {

    public enum Rules {
        CoopRace,
        Deathmatch
    }

    public Rules rules = Rules.CoopRace;

    public int numTeams = 2;
    public int numPlayers = 4;
    public int scoreLimit = 1000;
    public int timeLimit = 0;

    public Game.Scenes[] levelScenes;

}
