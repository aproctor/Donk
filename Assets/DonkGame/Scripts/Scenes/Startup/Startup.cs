using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Startup : MonoBehaviour {

  public float titleFadeTime = 2f;
  public float gameStartFadeTime = 2f;

  public Image instructionsImage;
  public Image curtain;

  public enum StartupState {
    Initial,
    TitleCard,
    LoadingInstructions,
    Instructions,
    LoadingGame
  }

  private StartupState state = StartupState.Initial;

  private float loadStartTime = 0f;

  // Use this for initialization
  void Start() {    
    Game.Initialize();
    this.state = StartupState.TitleCard;
  }

  void Update() {
    if(Input.anyKeyDown) {
      if(this.state == StartupState.TitleCard) {
        StartCoroutine(LoadInstructions());
      } else if(this.state == StartupState.Instructions) {
        StartCoroutine(LoadGame());
      }
    }

    if(this.state == StartupState.LoadingInstructions) {
      float alpha = Mathf.Lerp(0f, 1f, (Time.time - this.loadStartTime) / titleFadeTime);
      this.instructionsImage.color = new Color(this.instructionsImage.color.r, this.instructionsImage.color.g, this.instructionsImage.color.b, alpha);
    } else if(this.state == StartupState.LoadingGame) {
      float alpha = Mathf.Lerp(0f, 1f, (Time.time - this.loadStartTime) / gameStartFadeTime);
      this.curtain.color = new Color(this.curtain.color.r, this.curtain.color.g, this.curtain.color.b, alpha);
    }
  }

  private IEnumerator LoadInstructions() {
    this.state = StartupState.LoadingInstructions;
    this.loadStartTime = Time.time;

    yield return new WaitForSeconds(titleFadeTime);

    this.state = StartupState.Instructions;

    yield return null;
  }

  private IEnumerator LoadGame() {
    this.state = StartupState.LoadingGame;
    this.loadStartTime = Time.time;
    yield return new WaitForSeconds(gameStartFadeTime);

    Game.StartGame(Game.Config.modes[0]);

    yield return null;
  }
}
