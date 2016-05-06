using UnityEngine;
using System;
using System.Collections;

public abstract class Ability {

	protected Transform playerTransform;
	protected LayerMask mask;
	protected Player player;

	public Ability(Transform playerTransform, LayerMask layer, Player player) {
		this.playerTransform = playerTransform;
		this.mask = layer;
		this.player = player;
	}

    public abstract void Activate();

	protected void ApplyDamage(Damagable hitDamagable, int amount) {
		bool targetKilled = hitDamagable.TakeDamage(amount);
		if (targetKilled) {
			Player p = hitDamagable.GetComponent<Player>();
			if (p != null) {
				player.team.AddPoints (Game.round.mode.playerKillScoreAmount);
			}
			Chicken c = hitDamagable.GetComponent<Chicken>();
			if (c != null) {
				Debug.LogError ("Player? <"+player+">");
				Debug.LogError ("Team? <"+player.team+">");
				player.team.AddPoints(Game.round.mode.chickenKillScoreAmount);
			}
		}
	}

}
