using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

    [Header("Object Links")]
	public Text countdownText;	
	public Transform playerCameras;
    public Transform[] teamHuds;
	public GameOverUI gameOverUI;

    [Header("Prefab Links")]
    public GameObject playerHudPrefab;

	void Start() {
		this.gameOverUI.Hide();
	}

    #region cameras
    public void SetupCameras(Game.CameraMode cameraMode, Team[] teams) {

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
				SetupFollowCam(cameraConfig.cameraPrefabs[0], teams[0]);
				SetupFollowCam(cameraConfig.cameraPrefabs[1], teams[1]);
			} else if (cameraMode == Game.CameraMode.TwoByTwo) {
				Player[] tempPlayers = new Player[4] { teams[0].players[0], teams[0].players[1], teams[1].players[0], teams[1].players[1]};
				for (int i = 0; i < cameraConfig.cameraPrefabs.Length; i++) {
					SetupFollowCam (cameraConfig.cameraPrefabs [i], tempPlayers [i]);
				}

			} else if (cameraMode == Game.CameraMode.Single) {
				//TODO add first camera prefab to current player?
				Debug.LogError("Unimplemented Camera Mode");
			}

			//Spawn PiP UI
			SetupPipUI(cameraConfig.uiPrefab);
		}
	}


	//For a multi member camera
	private void SetupFollowCam(GameObject prefab, Team team) {
		
		GameObject newCamera = (GameObject)GameObject.Instantiate(prefab);
		newCamera.transform.SetParent(team.transform);
		FollowerCameraRig followerCamera = newCamera.GetComponent<FollowerCameraRig>();
		followerCamera.players = new Transform[team.players.Length];
		for (int i = 0; i < team.players.Length; i++) {
			followerCamera.players[i] = team.players[i].transform;
		}
	}

	private void SetupFollowCam(GameObject prefab, Player player) {
		GameObject newCamera = (GameObject)GameObject.Instantiate(prefab);
		newCamera.transform.SetParent(player.team.transform);
		FollowerCameraRig followerCamera = newCamera.GetComponent<FollowerCameraRig>();
		followerCamera.players = new Transform[1] { player.transform};
	}

	private void SetupPipUI(GameObject prefab) {
		GameObject pipUI = (GameObject)GameObject.Instantiate(prefab);        
		RectTransform rectTransform = pipUI.GetComponent<RectTransform>();
		rectTransform.SetParent(this.playerCameras);
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.one;
    }
    #endregion

    public void SetupPlayerHuds(Team[] teams) {
        
        if (teams.Length == 2) {
            for (int i = 0; i < teams.Length; i++) {
                Transform hudContainer = this.teamHuds[i];
                for (int j = 0; j < teams[i].players.Length; j++) {
                    GameObject hud = (GameObject)GameObject.Instantiate(playerHudPrefab);
                    RectTransform hudTransform = hud.GetComponent<RectTransform>();
                    hudTransform.SetParent(hudContainer, false);

                    //Not sure what layout we'll need in the future
                    //This forces team 1 left and team 2 right, and players on the top or bottom of their screen
                    hudTransform.pivot = new Vector2(i, j);
                    hudTransform.anchorMin = new Vector2(i, j);
                    hudTransform.anchorMax = new Vector2(i, j);
                    hudTransform.anchoredPosition = Vector3.zero;

                    hud.GetComponent<PlayerHud>().Init(teams[i].players[j]);
                }
            }
        }
    }

}
