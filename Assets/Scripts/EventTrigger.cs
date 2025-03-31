using System;
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
    private string objectTag = "Goal";

    [SerializeField] private float eventCooldown = 0;
    public float eventTimer = 0;
    
    [SerializeField]
    private UnityEvent event_Action;

    [SerializeField]
    private GameObject fadeOutCanvasPrefab;
    [SerializeField]
    private FadeController fadeController;

    private void Awake()
    {
        this.gameObject.tag = objectTag;
        eventTimer = eventCooldown;
    }

    private void Update()
    {
        eventTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (eventTimer <= eventCooldown)
        {
            return;
        }
        // Check if the collider is the player
        if (other.GetComponent<PlayerController>() != null || other.gameObject.CompareTag("Player"))
        {
            eventTimer = 0;
            // Invoke the UnityEvent if it has subscribers
            event_Action?.Invoke();

            // Change the scene if the changeScene flag is true
            if (changeScene)
            {
                GameObject fadeOut = Instantiate(fadeOutCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                fadeController = fadeOut.GetComponent<FadeController>();
                
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (fadeOutCanvasPrefab != null && fadeController.animationFinished)
        {
            Debug.Log("Animation finished!");

            LoadScene(sceneToLoad);
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