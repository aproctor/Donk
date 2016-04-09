using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class PlayerCameraRenderer : MonoBehaviour {

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
        //2x2 Layout, may want to abstract later for more screen
        int w = (int)(Screen.width / 2);
        int h = (int)(Screen.height / 2);

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
