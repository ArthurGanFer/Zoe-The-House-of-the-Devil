using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string nextLevel = "TestLevel")
    {
        SceneManager.LoadScene(nextLevel);
    }
}
