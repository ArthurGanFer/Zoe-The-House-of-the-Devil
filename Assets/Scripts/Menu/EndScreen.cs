using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    private SettingsManager settingsManager;    //A reference to our SettingsManager
    public string level;                        //A string representing the level we want to load

    private void Awake()
    {
        settingsManager = FindObjectOfType<SettingsManager>();
        settingsManager.currentCoins = 0;
    }

    public void Quit()
    {
        if (settingsManager != null)
        {
            settingsManager.QuitGame(true);
        }
        else
        {
            Debug.LogError("There is no SettingsManager in scene!");
        }
    }

    public void LoadLevel()
    {
        if (settingsManager != null)
        {
            settingsManager.LoadLevel(level);
        }
        else
        {
            Debug.LogError("There is no SettingsManager in scene!");
        }
    }
}
