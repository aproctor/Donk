using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

  public TeamObjects teamObjects;

  void Start() {
    Score = 0;
  }

  public int Score {
    get;
    private set;
  }

  /// <summary>
  /// Add points to the score based on objectives
  /// </summary>
  /// <returns>score</returns>
  public void ScoreObjectivePoints() {   
    AddPoints(this.teamObjects.chickenCoop.CountChickens() * Game.round.mode.chickenTickScoreAmount);
  }

  public void AddPoints(int points) {
    this.Score += points;
  }
}
