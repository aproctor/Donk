using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject {

    #region external_configs
    public ColorConfig colors;
    public CameraConfig[] cameras;
    public GameMode[] modes;
    #endregion


    public int maxPlayers = 4;

}
