using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject {

    #region external_configs
    public ColorConfig colors;
    public CameraConfig[] cameras;
    #endregion

    public GameMode[] modes;

    public LayerMask[] teamMask;

    public int maxPlayers = 4;

}
