using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;      // Assign the main camera in Inspector
    public Camera cutsceneCamera;  // Assign the cutscene camera in Inspector
    public float delay = 5f;       // Delay before switching cameras

    void Start()
    {
        if (mainCamera == null || cutsceneCamera == null)
        {
            Debug.LogError("Cameras not assigned in CameraSwitcher!");
            return;
        }

        cutsceneCamera.gameObject.SetActive(false); // Ensure cutscene camera is off at start
        StartCoroutine(SwitchCameraAfterDelay());
    }

    IEnumerator SwitchCameraAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        mainCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);

        Debug.Log("Switched to cutscene camera after " + delay + " seconds.");
    }
}
