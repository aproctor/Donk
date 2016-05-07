using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

	[HideInInspector]
  public TeamObjects teamObjects;
	[HideInInspector]
	public Player[] players;

	public LayerMask teamMask;

  void Start() {
    Score = 0;
	players = new Player[0];
  }

  public int Score {
    get;
    private set;
  }

	public void AddPlayer(Player p) {
		Player[] newPlayerList = new Player[players.Length + 1];
		for (int i = 0; i < players.Length; i++) {
			newPlayerList[i] = players[i];
		}
		newPlayerList [newPlayerList.Length - 1] = p;
		this.players = newPlayerList;

		p.transform.parent = this.transform;
		p.attackMask = teamMask;
    p.gameObject.layer = this.gameObject.layer;
		p.team = this;
	}

	public void AddGold(int amount) {
		for (int i = 0; i < this.players.Length; i++) {
			this.players[i].gold += amount;
		}
	}

  /// <summary>
  /// Add points to the score based on objectives
  /// </summary>
  /// <returns>score</returns>
  public void ScoreObjectivePoints() {   
    AddPoints(this.teamObjects.chickenCoop.CountChickens() * Game.round.mode.chickenTickScoreAmount);
    AddGold(this.teamObjects.chickenCoop.CountChickens() * Game.round.mode.chickenTickGoldAmount);
  }

  public void AddPoints(int points) {
    this.Score += points;
  }
}
