using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : GameState
{
    public GamePlay Play_State;
    public override GameState RunCurrentGameState()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Play!");
            return Play_State;
        }

        return this;
    }
}
