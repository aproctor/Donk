using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public Text countdownText;	
	public Transform playerCameras;

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
				//TODO add a camera to each player
//				for (int i = 0; i < cameraConfig.cameraPrefabs.Length; i++) {
//					((GameObject)GameObject.Instantiate(cameraConfig.cameraPrefabs[i])).transform.parent = team.players[i].transform;
//				}
				Debug.LogError("Unimplemented Camera Mode");
			} else if (cameraMode == Game.CameraMode.Single) {
				//TODO add first camera prefab to current player?
				Debug.LogError("Unimplemented Camera Mode");
			}

			//Spawn PiP UI
			SetupPipUI(cameraConfig.uiPrefab);
		}
	}

	private void SetupFollowCam(GameObject prefab, Team team) {
		
		GameObject newCamera = (GameObject)GameObject.Instantiate(prefab);
		newCamera.transform.SetParent(team.transform);
		FollowerCameraRig followerCamera = newCamera.GetComponent<FollowerCameraRig>();
		followerCamera.players = new Transform[team.players.Length];
		for (int i = 0; i < team.players.Length; i++) {
			followerCamera.players[i] = team.players[i].transform;
		}
	}

	private void SetupPipUI(GameObject prefab) {
		GameObject pipUI = (GameObject)GameObject.Instantiate(prefab);        
		RectTransform rectTransform = pipUI.GetComponent<RectTransform>();
		rectTransform.SetParent(this.playerCameras);
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.one;
	}
}
