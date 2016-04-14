using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class CameraConfig : ScriptableObject {

    public Game.CameraMode mode;
    public GameObject[] cameraPrefabs;
    public GameObject uiPrefab;
}
