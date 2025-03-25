using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    private GameManager gameManager;
    public Transform currentCheckpoint;
 

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("There is no GameObject of type GameManager in the scene!");
        }
        else
        {
            ResetSpawn();
        }
    }

    private void Update()
    {
        if (gameManager.playerSpawnPosition != currentCheckpoint.position || gameManager.playerSpawnRotation != currentCheckpoint.rotation)
        {
            gameManager.playerSpawnPosition = currentCheckpoint.position;
            gameManager.playerSpawnRotation = currentCheckpoint.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager != null && other.gameObject.tag == "CheckPoint")
        {
            SetCheckPoint(other.gameObject.transform);
        }
        if (gameManager != null && other.gameObject.tag == "Goal")
        {
            gameManager.spawnSet = false;
        }
    }

    private void ResetSpawn()
    {
        GameObject spawnObject = GameObject.FindGameObjectWithTag("SpawnPoint");
        
        if (spawnObject != null)
        {
            Transform spawnPoint = spawnObject.transform;

            currentCheckpoint = spawnPoint;

            if (!gameManager.spawnSet)
            {
                gameManager.playerSpawnPosition = currentCheckpoint.position;
                gameManager.playerSpawnRotation = currentCheckpoint.rotation;
                gameManager.spawnSet = true;
            }
            
            gameManager.SetSpawn();

            Debug.Log("Spawn has been set!");
        }
        else
        {
            Debug.Log($"There is no gameObject with the tag 'SpawnPoint' in {SceneManager.GetActiveScene().name}");
        }
    }

    private void SetCheckPoint(Transform checkPoint)
    {
        currentCheckpoint = checkPoint;

        Debug.Log("CheckPoint has been updated!");
    }
}
