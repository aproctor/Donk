using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class PlayerCameraRenderer : MonoBehaviour {

    public Game.CameraMode mode = Game.CameraMode.TwoByTwo;

    [SerializeField]
    private RawImage[] playerCameras;

    private float previousWidth = 0f;
    private float previousHeight = 0f;

    void Start() {
        SetupPlayerCameras();
    }
	
	// Update is called once per frame
	void Update () {
        if (ScreenHasChanged) {
            SetupPlayerCameras();
        }
	}

    private void SetupPlayerCameras() {
        int w = Screen.width;
        int h = Screen.height;

        if (this.mode == Game.CameraMode.TwoByTwo) {
            w = (int)(w / 2);
            h = (int)(h / 2);
        } else if (this.mode == Game.CameraMode.SideBySide) {
            w = (int)(w / 2);
        } else if (this.mode == Game.CameraMode.SideBySide) {
            h = (int)(h / 2);
        }

        for (int i = 0; i < playerCameras.Length; i++) {
            this.playerCameras[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            this.playerCameras[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        }

        previousHeight = Screen.height;
        previousWidth = Screen.width;
    }

    private bool ScreenHasChanged {
        get {
            return previousWidth != Screen.width || previousHeight != Screen.height;
        }
    }
}
