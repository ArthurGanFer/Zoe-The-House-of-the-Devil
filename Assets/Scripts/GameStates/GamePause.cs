using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : GameState
{
    public GameMenu Menu_State;
    public GamePlay Play_State;
    public override GameState RunCurrentGameState()
    {
        Time.timeScale = 0;
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Menu!");
            return Menu_State;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Play!");
            return Play_State;
        }

        return this;
    }
}
