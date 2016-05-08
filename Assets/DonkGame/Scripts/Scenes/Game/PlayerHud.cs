using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHud : MonoBehaviour {

	public Text goldText;
  public Text chargeText;
  private Player player;
	public Text abilityText;
  public Image abilityIconTop;
  public Image abilityIconBottom;

    public void Init(Player p) {
        this.player = p;
        this.goldText.color = p.PlayerColor;
        this.abilityIconTop.color = p.PlayerColor; 
    }
	
  // Update is called once per frame
  void Update() {
    if(player != null) {
      //Update gold text
      this.goldText.text = "$" + player.gold;

      this.abilityText.text = player.CurrentAbility.name;
      this.abilityIconTop.sprite = player.CurrentAbility.icon;
      this.abilityIconBottom.sprite = player.CurrentAbility.icon;
      this.abilityIconTop.fillAmount = player.CurrentAbility.CooldownPct;

      this.chargeText.text = player.CurrentAbility.chargesRemaining.ToString();
      this.chargeText.gameObject.SetActive(player.CurrentAbility.GetAbilityType() == Ability.Type.CONSUMABLE);

      //Update HP bar
      //Update selected items
    }	
  }

}
