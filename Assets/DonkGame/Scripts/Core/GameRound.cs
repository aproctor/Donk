using UnityEngine;
using System.Collections;

public class GameRound {

    public GameMode mode;

	public float startTime = 0f;
    
    public GameRound(GameMode mode) {        
        this.mode = mode;
    }
}
