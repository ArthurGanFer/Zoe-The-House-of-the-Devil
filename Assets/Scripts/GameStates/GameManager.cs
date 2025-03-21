using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public Vector3 playerSpawn;

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
            if (playerSpawn != null)
            {
                PlayerController[] players = FindObjectsOfType<PlayerController>();

                foreach (PlayerController player in players)
                {
                    if (player.mainCharacter == true)
                    {
                        player.transform.position = playerSpawn;

                        break;
                    }
                }
            }
            else
            {
                Debug.Log("There is no spawn point!");
            }
        }
        
    }
}
