using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;                   //Reference to our AudioMixer component
    public Camera mainCamera;                       //Reference to our Camera component
    public PostProcessLayer postProcessLayer;       //Reference to our PostProcessLayer component

    public bool monochrome;                         //A flag for if monochrome mode is on/off
    public float currentVolume;                     //A float keeping track of our current volume

    public int currentCoins;                       //An int keeping track of our current Coins        

    private static SettingsManager instance;        //Static instance of SettingsManager

    public static SettingsManager Instance
    {
        get { return instance; }
    }

    public int GetCurrentCoins()
    {
        return currentCoins;
    }

    void Awake()
    {
        AssignComponents();

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

    private void Update()
    {
        if (mainCamera == null)
        {
            AssignComponents();
            return;
        }

        if (monochrome)
        {
            postProcessLayer.enabled = true;
        }
        else
        {
            postProcessLayer.enabled = false;
        }
    }

    void AssignComponents()
    {
        if (mainCamera == null) 
        { 
            mainCamera = FindObjectOfType<Camera>();

            if (postProcessLayer == null)
            {
                postProcessLayer = mainCamera.GetComponent<PostProcessLayer>();
            }

            return; 
        }
        else
        {
            postProcessLayer = mainCamera.GetComponent<PostProcessLayer>();

            if (postProcessLayer == null)
            {
                Debug.LogError("PostProcessLayer component not found on Main Camera!");
                return;
            }
        }
    }

    public void ChangeCoins(int coinsValue)
    {
        currentCoins += coinsValue;
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

    public void MonochromeMode(bool on)
    {
        if (on)
        {
            monochrome = true;
        }
        if (!on)
        {
            monochrome = false;
        }
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        currentVolume = volume;
    }

    public void QuitGame(bool quit = false)
    {
        if (quit)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
