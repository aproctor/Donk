using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject {

    #region external_configs
    public ColorConfig colors;
    #endregion


    public int maxPlayers = 4;

}
