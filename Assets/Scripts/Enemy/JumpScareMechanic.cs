using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScareMechanic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject[] AvailablePrefabs;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Animator jumpScareAnim;
    private AudioSource audioSource;

    [Header("Properties")]
    [SerializeField] private GameObject activePrefab;
    [SerializeField] private string destinationScene;

    [Header("Jumpscare Sounds")]
    [SerializeField] private AudioClip dollJumpScareSound;
    [SerializeField] private AudioClip boyJumpScareSound;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add if missing
        }
    }

    private void Update()
    {
        if (jumpScareAnim != null && jumpScareAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            if (!string.IsNullOrEmpty(destinationScene))
            {
                SceneManager.LoadScene(destinationScene);
            }
            else
            {
                Debug.LogWarning("destinationScene has not been set!");
            }
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

            // **Play Jumpscare Sound Based on Enemy**
            PlayJumpscareSound(enemy.enemyName);
        }
        else
        {
            Debug.LogWarning($"No prefab found for enemy: {enemy.enemyName}");
        }
    }

    private void PlayJumpscareSound(string enemyName)
    {
        if (audioSource == null) return;

        AudioClip clip = null;

        if (enemyName == "Doll1")
        {
            clip = dollJumpScareSound;
        }
        else if (enemyName == "Boy")
        {
            clip = boyJumpScareSound;
        }

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"No jumpscare sound assigned for enemy: {enemyName}");
        }
    }
}
