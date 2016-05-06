using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameMode : ScriptableObject {

    public enum Rules {
        CoopRace,
		CaptureTheEgg,
        Deathmatch
    }

	public Rules rules = Rules.CaptureTheEgg;

    public int numTeams = 2;
    public int numPlayers = 4;
    public int scoreLimit = 1000;
    public int timeLimit = 0;

	public int eggScoreAmount = 0;
	public int playerKillScoreAmount = 0;
	public int chickenKillScoreAmount = 0;
	public int chickenTickScoreAmount = 0;

    public Game.Scenes[] levelScenes;

}
