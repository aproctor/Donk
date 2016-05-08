using UnityEngine;
using System;
using System.Collections;

public abstract class Ability : MonoBehaviour {

  public Sprite icon;

  public enum Type {
    CONSUMABLE,
    WEAPON
  }

  public bool HasCharges { get { return this.chargesRemaining > 0; } }
	public LayerMask mask { get; private set; }

  [SerializeField]
  protected float coolDown;
  [SerializeField]
  protected Type type;
  [SerializeField]
  GameObject model;

  protected Transform playerTransform;
	protected Player player;
  protected float lastUseTime = 0f;
  public int chargesRemaining { get; protected set; }
  public int stackQuantity = 1;

  public float CooldownPct {
    get {      
      return Mathf.Clamp((Time.time - this.lastUseTime) / this.coolDown, 0f, 1f);
    }
  }

	public virtual void Init(Transform playerTransform, LayerMask layer, Player player) {
		this.playerTransform = playerTransform;
		this.mask = layer;
		this.player = player;
    this.chargesRemaining = 1;

    if(this.model != null) {
      this.model.transform.position = player.rightHandTransform.position;
      this.model.transform.parent = this.player.rightHandTransform;
    }
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

  public Type GetAbilityType() {
    return this.type;
  }

  public virtual void AddQuantity(int value) {
    this.chargesRemaining += value;
  }

  public virtual void OnSelected() {
    if(this.model != null) {
      this.model.SetActive(true);
    }
  }

  public virtual void OnDeselected() {
    if (this.model != null) {
      this.model.SetActive(false);
    }
  }

  protected virtual bool OnCooldown() {
    return ((Time.time - this.lastUseTime) < this.coolDown);
  }

}
