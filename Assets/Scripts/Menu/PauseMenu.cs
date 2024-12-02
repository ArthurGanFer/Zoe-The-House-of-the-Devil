using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;              //A reference to our Pause Menu GameObject
    private SettingsManager settingsManager;    //A reference to our SettingsManager

    public static bool gameIsPaused = false;    //A flag for if the game is paused

    private void Awake()
    {
        settingsManager = FindObjectOfType<SettingsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void MonochromeMode(bool on)
    {
        if (on)
        {
            settingsManager.MonochromeMode(true);
        }
        else
        {
            settingsManager.MonochromeMode(false);
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void SetVolume(float volume)
    {
        if (settingsManager != null)
        {
            settingsManager.SetVolume(volume);
        }
        else
        {
            Debug.LogError("There is no SettingsManager in scene!");
        }
    }
}
