using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMaster : MonoBehaviour {
    
    [Header("Object Links")]
    [SerializeField]
    private GameObject ui;

    //TODO spawn these dynamically
    public GameObject[] teamObjs;
    public GameObject[] playerObjs;

	// Use this for initialization
	void Start () {
        Game.Initialize();

        if (Game.round == null) {
            LoadGameDefaults();
        }

        LoadLevels();


        //TODO Spawn Players and setup input devices

	}

    private void LoadGameDefaults() {
        Debug.LogWarning("Loading Game Defaults, this should have been setup by the menu scene");
        Game.round = new GameRound(Game.Config.modes[0]);
    }

    /**
     * Based on the game round level info, load up the required scenes
     */
    private void LoadLevels() {
        for (int i = 0; i < Game.round.mode.levelScenes.Length; i++) {
#if UNITY_EDITOR
            //For ease of development, scenes that are loaded additively might have to be disposed
            //This apparently works with unsaved scenes, which is pretty awesome.
            SceneManager.UnloadScene((int)Game.round.mode.levelScenes[i]);
#endif
            SceneManager.LoadScene((int)Game.round.mode.levelScenes[i], LoadSceneMode.Additive);
        }
    }









    #region camera_crap_cleanup
    //FIXME clean up this camera spawning logic once we have some decisions on how we want to handle cameras
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
                SetupFollowCam(cameraConfig.cameraPrefabs[0], teamObjs[0], playerObjs[0], playerObjs[1]);
                SetupFollowCam(cameraConfig.cameraPrefabs[1], teamObjs[1], playerObjs[2], playerObjs[3]);
            } else if (cameraMode == Game.CameraMode.TwoByTwo) {
                //TODO add a camera to each player
                for (int i = 0; i < cameraConfig.cameraPrefabs.Length; i++) {
                    ((GameObject)GameObject.Instantiate(cameraConfig.cameraPrefabs[i])).transform.parent = playerObjs[i].transform;
                }
            } else if (cameraMode == Game.CameraMode.Single) {
                //TODO add first camera prefab to current player?
                Debug.LogError("Unimplemented Camera Mode");
            }

            //Spawn PiP UI
            SetupPipUI(cameraConfig.uiPrefab);
        }
    }

    private void SetupFollowCam(GameObject prefab, GameObject team, GameObject player1, GameObject player2) {
        GameObject newCamera = (GameObject)GameObject.Instantiate(prefab);
        newCamera.transform.SetParent(team.transform);
        FollowerCameraRig followerCamera = newCamera.GetComponent<FollowerCameraRig>();
        followerCamera.player1 = player1.transform;
        followerCamera.player2 = player2.transform;
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
