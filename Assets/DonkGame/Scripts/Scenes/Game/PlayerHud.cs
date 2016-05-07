using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHud : MonoBehaviour {

	public Text goldText;
    private Player player;
	public Text abilityText;

    public void Init(Player p) {
        this.player = p;
        this.goldText.color = p.PlayerColor;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null) {
            //Update gold text
            this.goldText.text = "$" + player.gold;

			this.abilityText.text = player.CurrentAbility.name;

            //Update HP bar
            //Update selected items
        }	
	}
}
