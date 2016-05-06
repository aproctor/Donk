using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject {

    #region external_configs
    public ColorConfig colors;
    public CameraConfig[] cameras;
    #endregion

    public GameMode[] modes;

    public int maxPlayers = 4;

}
