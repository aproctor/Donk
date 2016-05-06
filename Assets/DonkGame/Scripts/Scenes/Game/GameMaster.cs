using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMaster : MonoBehaviour {

    #region attributes
    [Header("Object Links")]
    [SerializeField]
    private GameObject ui;

    //TODO spawn these dynamically
    public Team[] teams;
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
	void Start () {
        this.state = GameMasterState.Setup;
        Game.Initialize();

        if (Game.round == null) {
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
        for (int i = 0; i < Game.round.mode.levelScenes.Length; i++) {
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

    int playerIndex = 0;
    for(int i = 0; i < Game.round.mode.numTeams; i++) {
      TeamObjects teamObjs = level.teamObjects[i];
      this.teams[i].teamObjects = teamObjs;
      teamObjs.chickenCoop.team = this.teams[i];

      for(int j = 0; j < numPlayersPerTeam; j++) {
        Transform spawnPoint = teamObjs.spawnPoints[j];
        Player player = ((GameObject)GameObject.Instantiate(this.playerPrefab)).GetComponent<Player>();
        player.playerNumber = playerIndex + 1;       
        player.transform.position = spawnPoint.transform.position;
        player.transform.parent = this.teams[i].transform;
        player.gameObject.name = "Player " + player.playerNumber;
        player.attackMask = Game.Config.teamMask[i];
        this.players[playerIndex] = player;

        ++playerIndex;
      }
    }
    SetupCameras(Game.CameraMode.SideBySide);

        this.state = GameMasterState.WaitingForPlayers;
    }

    #endregion


    #region update_methods

    void Update() {
        if (this.state == GameMasterState.Playing) {
            UpdatePlayState();
        } else if (this.state == GameMasterState.WaitingForPlayers) {
            CheckForMinimumPlayers();
        } else if (this.state == GameMasterState.Playing) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                PauseGame();
            }
        } else if (this.state == GameMasterState.Paused) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                UnPauseGame();
            }
        } else if (this.state == GameMasterState.GameOver) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
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
        for (int i = 0; i < this.secondsToStart; i++) {
            Debug.LogError(this.secondsToStart - i);
            yield return oneSecondDelay;
        }

		Game.round.startTime = Time.time;
        ResumeGame();

        yield return null;
    }

    public void PauseGame() {
        Debug.LogError("PAUSED");
        this.state = GameMasterState.Paused;
    }
    public void UnPauseGame() {
        StartCoroutine(StartGameCountdown());
    }

    private void ResumeGame() {
        this.state = GameMasterState.Playing;
        Debug.LogError("GO!");
    }

    
    private void UpdatePlayState() {
        //Tick Scores
        if (Time.time - this.lastScoreTickTime > this.gameScoreTickRate) {
            for (int i = 0; i < this.teams.Length; i++) {
                this.teams[i].ScoreObjectivePoints();
            }
            this.lastScoreTickTime = Time.time;
        }

        //Check Scores
        for (int i = 0; i < this.teams.Length; i++) {
            if (this.teams[i].Score > Game.round.mode.scoreLimit) {                
                GameOver();
            }
        }
    }

    private void GameOver() {
        Debug.LogError("Game Over");
        this.state = GameMasterState.GameOver;
    }

    #endregion 



    #region setup_cameras
    private void SetupCameras(Game.CameraMode cameraMode) {
        CameraConfig cameraConfig = null;
        for (int i = 0; i < Game.Config.cameras.Length; i++) {
            if (Game.Config.cameras[i].mode == cameraMode) {
                cameraConfig = Game.Config.cameras[i];
                break;
            }
        }

        if (cameraConfig == null) {
            Debug.LogError("Unable to find config for camera mode <" + cameraMode.ToString() + ">");
        } else {
            //Spawn cameras based on mode
            if (cameraMode == Game.CameraMode.SideBySide || cameraMode == Game.CameraMode.TwoStacked) {
                //Add a camera to each team
                SetupFollowCam(cameraConfig.cameraPrefabs[0], teams[0], players[0], players[1]);
                SetupFollowCam(cameraConfig.cameraPrefabs[1], teams[1], players[2], players[3]);
            } else if (cameraMode == Game.CameraMode.TwoByTwo) {
                //TODO add a camera to each player
                for (int i = 0; i < cameraConfig.cameraPrefabs.Length; i++) {
                    ((GameObject)GameObject.Instantiate(cameraConfig.cameraPrefabs[i])).transform.parent = players[i].transform;
                }
            } else if (cameraMode == Game.CameraMode.Single) {
                //TODO add first camera prefab to current player?
                Debug.LogError("Unimplemented Camera Mode");
            }

            //Spawn PiP UI
            SetupPipUI(cameraConfig.uiPrefab);
        }
    }

    private void SetupFollowCam(GameObject prefab, Team team, Player player1, Player player2) {
        GameObject newCamera = (GameObject)GameObject.Instantiate(prefab);
        newCamera.transform.SetParent(team.transform);
        FollowerCameraRig followerCamera = newCamera.GetComponent<FollowerCameraRig>();
        followerCamera.players[0] = player1.transform;
        followerCamera.players[1] = player2.transform;
    }

    private void SetupPipUI(GameObject prefab) {
        GameObject pipUI = (GameObject)GameObject.Instantiate(prefab);        
        RectTransform rectTransform = pipUI.GetComponent<RectTransform>();
        rectTransform.SetParent(this.ui.GetComponent<RectTransform>());
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.one;
    }
    #endregion

}
