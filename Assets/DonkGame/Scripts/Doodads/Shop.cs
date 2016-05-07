using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Shop : MonoBehaviour {

	public int goldCost = 0;
	public GameObject abilityPrefab;
	public float shopCooldown = 10f;

	public bool open = true;
	private float lastPurchaseTime = 0f;

	[Header("Object Links")]
	[SerializeField]
	private Text costIndicator;
	[SerializeField]
	private Image cooldownIndicator;

	[Header("Events")]
	public UnityEvent OnPurchase;
	public UnityEvent OnShopReady;

	void Start() {
		if (goldCost != 0) {
			costIndicator.text = "$" + goldCost;
		} else {
			costIndicator.gameObject.SetActive(false);
		}
		this.cooldownIndicator.gameObject.SetActive(false);
	}

	void Update() {
		if (!open) {
			if (Time.time > lastPurchaseTime + shopCooldown) {
				OpenShop();
			} else {
				this.cooldownIndicator.fillAmount = (Time.time - lastPurchaseTime) / shopCooldown;

			}
		}
	}

	public void ItemPurchased() {
		open = false;
		lastPurchaseTime = Time.time;
		this.cooldownIndicator.fillAmount = 0;
		this.cooldownIndicator.gameObject.SetActive(true);

		OnPurchase.Invoke();
	}

	private void OpenShop() {
		this.cooldownIndicator.gameObject.SetActive(false);
		this.open = true;
		OnShopReady.Invoke();	
	}
}
