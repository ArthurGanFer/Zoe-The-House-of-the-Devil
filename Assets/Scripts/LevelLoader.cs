using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadPreviousLevel()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
        {
            SceneManager.LoadScene(gameManager.previousLevelName);
        }
    }
}
