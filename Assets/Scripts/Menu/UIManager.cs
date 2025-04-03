using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    private static UIManager instance;

    public static UIManager Instance
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
    
    public void LoadLevel(string nextLevel = "TestLevel")
    {
        if (nextLevel != null)
        {
            GameManager.Instance.LoadLevel(nextLevel); }
        else
        {
            Debug.LogError("Level is not assigned!");
        }
    }

    public void LoadPreviousLevel()
    {
        GameManager.Instance.LoadLevel(GameManager.Instance.previousLevelName);
    }

    public void SetVolume (float volume)
    {
        //Audio Volume adjust
    }
    
}
