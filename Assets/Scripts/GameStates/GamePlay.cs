using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : GameState
{
    public GamePause Pause_State;
    public override GameState RunCurrentGameState()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pause");
            return Pause_State;
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            DialogManager.instance.PlayCurrentSpeech();
        }

        return this;
    }
}
