using UnityEngine;
using System;
using System.Collections;

public abstract class Ability : MonoBehaviour {

	public LayerMask mask { get; private set; }

  [SerializeField]
  protected float coolDown;

	protected Transform playerTransform;
	protected Player player;
  protected float lastUseTime = 0f;

	public virtual void Init(Transform playerTransform, LayerMask layer, Player player) {
		this.playerTransform = playerTransform;
		this.mask = layer;
		this.player = player;
	}

  public abstract void Activate();

	public void ApplyDamage(Damagable hitDamagable, int amount) {
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

  protected virtual bool OnCooldown() {
    return ((Time.time - this.lastUseTime) < this.coolDown);
  }

}
