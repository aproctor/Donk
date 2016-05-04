using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

    void Start() {
        Score = 0;
    }

    public int Score {
        get; private set;
    }

    /// <summary>
    /// Add points to the score based on objectives
    /// </summary>
    /// <returns>score</returns>
    public void ScoreObjectivePoints() {
        //TODO check pens for points, using random score per tick
        Score += Random.Range(1, 5);
    }
}
