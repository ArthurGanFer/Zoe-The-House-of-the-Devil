using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;
    public bool invertedCamera = true;

    public static SettingsManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActiveInvertCamera(bool value)
    {
        invertedCamera = value;
    }
    
    public void QuitGame(bool quit = false)
    {
        if (quit)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
