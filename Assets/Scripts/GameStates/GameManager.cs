using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public Vector3 playerSpawnPosition;
    public Quaternion playerSpawnRotation;
    private bool playerInScene = false;
    public bool spawnSet;
    public Animator detectionAnim;
    [SerializeField]
    private GameObject fadeInCanvasPrefab;
    public string previousLevelName;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        RunGameStateMachine();

        if (playerInScene)
        {
            if (FindObjectOfType<EnemyController>() != null)
            {
                CheckIfPlayerInSight();
            }
        }
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
        Instantiate(fadeInCanvasPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        if (FindObjectOfType<PlayerController>() != null)
        {
            if (SceneManager.GetActiveScene().name != previousLevelName)
            {
                previousLevelName = SceneManager.GetActiveScene().name;
            }

            playerInScene = true;

            if (playerSpawnPosition != null && playerSpawnRotation != null)
            {
                SetSpawn();
            }
            else
            {
                Debug.Log("There is no spawn point!");
            }
        }
        else
        {
            playerInScene = false;

            Debug.Log($"There are no gameObjects of type PlayerController in scene: {SceneManager.GetActiveScene().name}");
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

    private void CheckIfPlayerInSight()
    {
        if (detectionAnim == null)
        {
            GameObject detectionGameObject = GameObject.FindGameObjectWithTag("DetectionIcon");
            detectionAnim = detectionGameObject.GetComponent<Animator>();
        }
        else
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();

            foreach (EnemyController enemy in enemies)
            {
                if (enemy.jumpScareAsset)
                {
                    detectionAnim.gameObject.SetActive(false);

                    break;
                }
                else
                {
                    detectionAnim.gameObject.SetActive(true);

                    if (enemy.playerInSight)
                    {
                        if (!enemy.chasingPlayer)
                        {
                            detectionAnim.SetInteger("DetectionLevel", 2);
                        }
                        else
                        {
                            detectionAnim.SetInteger("DetectionLevel", 3);
                        }

                        break;
                    }
                    else
                    {
                        detectionAnim.SetInteger("DetectionLevel", 1);
                    }
                }
            }
        }
        
    }
}
