using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;                   //Reference to our AudioMixer component
    public Camera mainCamera;                       //Reference to our Camera component
    private PostProcessLayer postProcessLayer;      //Reference to our PostProcessLayer component

    private static SettingsManager instance;        //Static instance of SettingsManager

    static SettingsManager Instance
    {
        get { return instance; }
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

    void AssignComponents()
    {
        if (mainCamera == null) 
        { 
            Debug.LogError("Main Camera is not assigned!"); 
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

    public void LoadLevel(string nextLevel = "TestLevel")
    {
        if (nextLevel != null)
        {
            SceneManager.LoadScene(nextLevel);
            AssignComponents();
        }
        else
        {
            Debug.LogError("Level is not assigned!");
        }
    }

    public void MonochromeMode(bool monochrome = false)
    {
        postProcessLayer.enabled = monochrome;
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
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
