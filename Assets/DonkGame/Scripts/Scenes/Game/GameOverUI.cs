using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	public Text messageText;

	public void Show(string message, Color teamColor) {
		messageText.text = message;
		messageText.color = teamColor;
		this.gameObject.SetActive(true);
	}

	public void Hide() {
		this.gameObject.SetActive(false);
	}
}
