using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("There is no GameObject of type GameManager in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager != null && other.gameObject.tag == "SpawnPoint")
        {
            Vector3 spawnPoint = other.gameObject.transform.position;

            if (spawnPoint != gameManager.playerSpawn)
            {
                gameManager.playerSpawn = spawnPoint;
            }
        }
    }
}
