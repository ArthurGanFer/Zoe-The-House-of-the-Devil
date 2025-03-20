using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour
{
    public bool changeScene = false;
    public string sceneToLoad = "Boy_Bedroom"; // Set the default scene to load

    [SerializeField]
    private UnityEvent event_Action;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the player
        if (other.GetComponent<PlayerController>() != null || other.gameObject.CompareTag("Player"))
        {
            // Invoke the UnityEvent if it has subscribers
            event_Action?.Invoke();

            // Change the scene if the changeScene flag is true
            if (changeScene)
            {
                LoadScene(sceneToLoad);
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        // Ensure the scene name is not empty or null
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Load the scene using SceneManager
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not specified.");
        }
    }
}