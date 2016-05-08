using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;

public class GameMaster : MonoBehaviour {

  #region attributes

  [Header("Object Links")]
  [SerializeField]
  private UIController ui;

  //TODO spawn these dynamically
  public Team[] teams;
  [HideInInspector]
  public Player[] players;

  private enum GameMasterState {
    Setup,
    LoadingLevel,
    WaitingForPlayers,
    Countdown,
    Playing,
    Paused,
    GameOver,
    Exiting
  }

  private GameMasterState state = GameMasterState.Setup;

  public int secondsToStart = 3;
  //Optimization to avoid runtime allocation, reuse wait for seconds definition
  private WaitForSeconds oneSecondDelay = new WaitForSeconds(1f);

  public float gameScoreTickRate = 1f;
  private float lastScoreTickTime = 0f;

  [Header("Prefabs")]
  [SerializeField]
  private GameObject playerPrefab = null;


  #endregion


  #region setup_methods

  // Use this for initialization
  void Start() {
    this.state = GameMasterState.Setup;
    Game.Initialize();

    if(Game.round == null) {
      LoadGameDefaults();
    }

    LoadLevels();
    //When Levels loaded is ready, the process will continue
  }

  private void LoadGameDefaults() {
    Debug.LogWarning("Loading Game Defaults, this should have been setup by the menu scene");
    Game.round = new GameRound(Game.Config.modes[0]);
  }

  /**
     * Based on the game round level info, load up the required scenes
     */
  private void LoadLevels() {
    this.state = GameMasterState.LoadingLevel;
    for(int i = 0; i < Game.round.mode.levelScenes.Length; i++) {
#if UNITY_EDITOR
      //For ease of development, scenes that are loaded additively might have to be disposed
      //This apparently works with unsaved scenes, which is pretty awesome.
      SceneManager.UnloadScene((int)Game.round.mode.levelScenes[i]);
#endif
      SceneManager.LoadScene((int)Game.round.mode.levelScenes[i], LoadSceneMode.Additive);
    }        
  }

  public void LevelLoaded(LevelConfig level) {
    //TODO Link up relevant game objects from level config
    int numPlayersPerTeam = Game.round.mode.numPlayers / Game.round.mode.numTeams;

    //Spawn Players
    this.players = new Player[Game.round.mode.numPlayers];
    int playerIndex = 0;
    for(int i = 0; i < Game.round.mode.numTeams; i++) {
      TeamObjects teamObjs = level.teamObjects[i];
      this.teams[i].teamObjects = teamObjs;
      teamObjs.chickenCoop.team = this.teams[i];

      for(int j = 0; j < numPlayersPerTeam; j++) {
        Transform spawnPoint = teamObjs.spawnPoints[j];

        Player player = ((GameObject)GameObject.Instantiate(this.playerPrefab)).GetComponent<Player>();
        player.playerNumber = playerIndex + 1;       
        player.gameObject.name = "Player " + player.playerNumber;
        player.transform.position = spawnPoint.transform.position;        
        player.spawnPoint = spawnPoint.transform.position;
        player.PlayerColor = Game.Config.colors.teams[i].colors[j];
        player.gold = Game.round.mode.playerStartingGold;
        this.players[playerIndex] = player;
        this.teams[i].AddPlayer(player);
        player.canMove = false;

        ++playerIndex;
      }
    }
    this.ui.SetupCameras(Game.Config.preferedCamMode, this.teams);
    this.ui.SetupPlayerHuds(this.teams);
    this.ui.SetupTeamScores(this.teams);

    this.state = GameMasterState.WaitingForPlayers;
  }

  #endregion


  #region update_methods

  void Update() {
    #if UNITY_EDITOR
    this.gameObject.name = "Game Master[" + this.state.ToString() + "]";
    #endif

    if(this.state == GameMasterState.Playing) {
      UpdatePlayState();
    } else if(this.state == GameMasterState.WaitingForPlayers) {
      CheckForMinimumPlayers();
    } else if(this.state == GameMasterState.Paused) {

      bool anyDeviceButtonPressed = false;
      var devices = InputManager.Devices;
      for(int i = 0; i < devices.Count; ++i) {
        if(devices[i].AnyButton) {
          anyDeviceButtonPressed = true;
          break;
        }
      }

      if(Input.GetKeyDown(KeyCode.Escape)) {
        this.QuitGame();
      } else if((InputManager.AnyKeyIsPressed && !(Input.GetKey(KeyCode.Escape))) || anyDeviceButtonPressed) {
        this.UnPauseGame();
      }
    } else if(this.state == GameMasterState.GameOver) {
      if(Input.anyKeyDown) {
        //Go to Main Menu
        this.state = GameMasterState.Exiting;
        SceneManager.LoadScene((int)Game.Scenes.Startup);
      }
    } 
  }

  private void CheckForMinimumPlayers() {
    //TODO check if joined players >= min players for a round
    StartCoroutine(StartGameCountdown());
  }

  IEnumerator StartGameCountdown() {
    this.state = GameMasterState.Countdown;
    for(int i = 0; i < this.secondsToStart; i++) {
      this.ui.countdownText.text = (this.secondsToStart - i).ToString();
      yield return oneSecondDelay;
    }

    Game.round.startTime = Time.time;
    ResumeGame();

    this.ui.countdownText.text = "GO!";

    yield return oneSecondDelay;

    this.ui.countdownText.text = "";        

    yield return null;
  }

  public void PauseGame() {
    this.ui.countdownText.text = "PAUSED";        
    this.state = GameMasterState.Paused;
  }

  public void UnPauseGame() {
    StartCoroutine(StartGameCountdown());
  }

  private void ResumeGame() {
    this.state = GameMasterState.Playing;

    for(int i = 0; i < this.teams.Length; i++) {
      for(int j = 0; j < this.teams[i].players.Length; j++) {
        this.teams[i].players[j].canMove = true;
      }
    }
  }

  private void QuitGame() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }

  private void UpdatePlayState() {
    if(Input.GetKeyDown(KeyCode.Escape) || this.StartButtonPressed) {
      PauseGame();
    }

    //Tick Scores
    if(Time.time - this.lastScoreTickTime > this.gameScoreTickRate) {
      for(int i = 0; i < this.teams.Length; i++) {
        this.teams[i].ScoreObjectivePoints();
      }
      this.lastScoreTickTime = Time.time;
    }

    //Check Scores
    for(int i = 0; i < this.teams.Length; i++) {
      this.ui.teamScoreLabels[i].text = this.teams[i].Score.ToString();
      if(this.teams[i].Score >= Game.round.mode.scoreLimit) {                
        GameOver(this.teams[i]);
      }
    }
  }

  private void GameOver(Team winner) {
    for(int i = 0; i < this.teams.Length; i++) {
      for(int j = 0; j < this.teams[i].players.Length; j++) {
        this.teams[i].players[j].canMove = false;
      }
    }
      
    this.state = GameMasterState.GameOver;
    this.ui.gameOverUI.Show(winner.gameObject.name + " Wins", winner.players[0].PlayerColor);
  }

  #endregion


  private bool StartButtonPressed {
    get {
      var devices = InputManager.Devices;
      for(int i = 0; i < devices.Count; ++i) {
        if(devices[i].MenuWasPressed) {
          return true;
          break;
        }
      }

      return false;
    }
  }



}
