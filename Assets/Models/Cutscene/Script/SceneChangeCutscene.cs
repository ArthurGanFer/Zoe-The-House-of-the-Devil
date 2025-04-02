using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeCutscene : MonoBehaviour
{
    public string sceneName; // Nome da cena para carregar

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}


