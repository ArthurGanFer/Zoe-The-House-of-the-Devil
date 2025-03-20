using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScareMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    [Tooltip("A reference to our array of prefab GameObjects")]
    private GameObject[] AvailablePrefabs;
    [SerializeField]
    [Tooltip("A reference to our main camera")]
    private GameObject mainCamera;
    [SerializeField]
    [Tooltip("A reference to our Animator")]
    private Animator jumpScareAnim;

    [Space(10)]
    [Header("Properties")]
    [SerializeField]
    [Tooltip("A flag for which of our prefabs is active")]
    private GameObject activePrefab;
    [SerializeField]
    [Tooltip("A reference to which scene we're transitioning to")]
    private string destinationScene;

    private void OnEnable()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera == null)
        {
            Debug.Log("There is no GameObject tagged as MainCamera in the scene!");
        }
    }

    private void Update()
    {
        if (jumpScareAnim != null && jumpScareAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            Debug.Log("Animation has finished!");
            SceneManager.LoadScene(destinationScene);
        }
    }

    public void CreateJumpScare(EnemyController enemy, int model, GameObject player)
    {
        foreach (GameObject prefab in AvailablePrefabs)
        {
            if (prefab.GetComponentInChildren<EnemyController>().enemyName == enemy.enemyName)
            {
                activePrefab = prefab;

                break;
            }
        }

        if (activePrefab != null)
        {
            player.SetActive(false);
            mainCamera.SetActive(false);

            GameObject jumpScare = Instantiate(activePrefab, new Vector3(0, 5000, 0), Quaternion.identity);

            jumpScareAnim = jumpScare.GetComponent<Animator>();
            jumpScareAnim.SetBool("isActive", true);
            jumpScareAnim.SetFloat("modelNumber", model);
            
        }
        else
        {
            Debug.Log($"There is no prefab containing an enemy named {enemy.enemyName}");
        }

    }
}
