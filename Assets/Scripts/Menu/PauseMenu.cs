using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;              //A reference to our Pause Menu GameObject
    private SettingsManager settingsManager;    //A reference to our SettingsManager

    public static bool gameIsPaused = false;    //A flag for if the game is paused
    [SerializeField]
    private GameObject onButton;                //A reference to our on button GameObject
    [SerializeField]
    private GameObject offButton;               //A reference to our off button GameObject
    [SerializeField]
    private Slider volume;                      //A reference to our volume Slider
    [SerializeField]
    public TMP_Text coinText;

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

        if (settingsManager != null)
        {
            if (settingsManager.monochrome)
            {
                onButton.SetActive(true);
                offButton.SetActive(false);
            }
            else
            {
                onButton.SetActive(false);
                offButton.SetActive(true);
            }

            volume.value = settingsManager.currentVolume;
            DisplayCoinCount(settingsManager.currentCoins);
        }
    }

    private void DisplayCoinCount(int coinCount)
    {
        coinText.text = string.Format("{00}", coinCount);
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
