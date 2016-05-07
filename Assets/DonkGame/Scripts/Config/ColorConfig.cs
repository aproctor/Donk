using UnityEngine;
using System.Collections;

public class ColorConfig : ScriptableObject {

    [System.Serializable]
    public struct TeamColors {
        public Color[] colors;
    }

    public TeamColors[] teams;
}
