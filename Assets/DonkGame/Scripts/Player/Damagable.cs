using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Damagable : MonoBehaviour {

    public float maxHealth = 100f;

	private float currentHealth = 100f;
	public float HealthPercent {
		get {
			return this.currentHealth / this.maxHealth;
		}	
	}
    

    public UnityEvent OnRecieveDamage;

    public UnityEvent OnDie;

  public UnityEvent OnReset;

    void Start() {
        this.currentHealth = this.maxHealth;
    }

    public void Reset() {
        this.currentHealth = maxHealth;
        this.OnReset.Invoke();
    }

	public void Die() {
		this.TakeDamage(this.currentHealth); 
    }

    /**
     * Returns whether the damage dealt killed the item
     */
    public bool TakeDamage(float damageAmount) {
        if (this.IsAlive) {
            this.currentHealth = Mathf.Max(0f, this.currentHealth - damageAmount);
            this.OnRecieveDamage.Invoke();
            if (Mathf.Approximately(0f, this.currentHealth)) {
                OnDie.Invoke();
                return true;
            }

        }
        return false;
    }

    public bool IsAlive {
        get {
            return !Mathf.Approximately(0f, this.currentHealth);
        }
    }

    void Update() {  
    }
}
