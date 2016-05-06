using UnityEngine;
using System;
using System.Collections;

public abstract class Ability : MonoBehaviour {

	protected Transform playerTransform;
	protected LayerMask mask;
	protected Player player;

	public virtual void Init(Transform playerTransform, LayerMask layer, Player player) {
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
				player.gold += Game.round.mode.playerKillGoldAmount;
			}
			Chicken c = hitDamagable.GetComponent<Chicken>();
			if (c != null) {
				player.team.AddPoints(Game.round.mode.chickenKillScoreAmount);
				player.gold += Game.round.mode.chickenKillGoldAmount;
			}
		}
	}

}
