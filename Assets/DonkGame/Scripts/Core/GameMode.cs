using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameMode : ScriptableObject {

    public enum Rules {
        CoopRace,
		CaptureTheEgg,
        Deathmatch
    }

	[Header("Level Data")]
	public Rules rules = Rules.CaptureTheEgg;
	public Game.Scenes[] levelScenes;
    public int numTeams = 2;
    public int numPlayers = 4;

	[Header("Win Conditions")]
    public int scoreLimit = 1000;
    public int timeLimit = 0;

	[Header("Score")]
	public int eggCaptureScoreAmount = 0;
	public int playerKillScoreAmount = 0;
	public int chickenKillScoreAmount = 0;
	public int chickenTickScoreAmount = 0;

	[Header("Gold")]
	public int eggCaptureGoldAmount = 0;
	public int playerKillGoldAmount = 0;
	public int chickenKillGoldAmount = 0;
	public int chickenTickGoldAmount = 0;

    

}
