using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public Vector3 playerSpawnPosition;
    public Quaternion playerSpawnRotation;
    public bool spawnSet;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public GameState Current_Game_State;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        RunGameStateMachine();
    }

    private void RunGameStateMachine()
    {
        GameState next_state = Current_Game_State?.RunCurrentGameState();

        if (next_state != null)
        {
            SwitchToNextState(next_state);
        }
    }

    public void SwitchToNextState(GameState next_state)
    {
        Current_Game_State = next_state;
    }

    public void LoadLevel(string nextLevel = "TestLevel")
    {
        if (nextLevel != null)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.LogError("Level is not assigned!");
        }
    }

    public void QuitGame(bool quit = false)
    {
        if (quit)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (FindObjectOfType<PlayerController>().mainCharacter == true)
        {
            if (playerSpawnPosition != null && playerSpawnRotation != null)
            {
                SetSpawn();
            }
            else
            {
                Debug.Log("There is no spawn point!");
            }
        }

    }

    public void SetSpawn()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        foreach (PlayerController player in players)
        {
            if (player.mainCharacter == true)
            {
                player.transform.position = playerSpawnPosition;
                player.transform.rotation = playerSpawnRotation;

                Debug.Log($"Player has been teleported to {playerSpawnPosition}");

                break;
            }
        }
    }
}
