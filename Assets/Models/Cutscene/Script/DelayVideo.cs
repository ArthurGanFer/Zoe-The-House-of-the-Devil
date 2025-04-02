using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DelayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector
    public float delay = 3f; // Delay before playing the video

    public Renderer tvRenderer; // The Renderer of the TV
    public Material defaultMaterial; // Material before video starts
    public Material videoMaterial; // Material with Render Texture

    void Start()
    {
        if (videoPlayer == null || tvRenderer == null || defaultMaterial == null || videoMaterial == null)
        {
            Debug.LogError("Missing references in DelayVideo script!");
            return;
        }

        videoPlayer.playOnAwake = false; // Ensure video does not play automatically
        tvRenderer.material = defaultMaterial; // Set the default material

        StartCoroutine(PlayVideoAfterDelay());
    }

    IEnumerator PlayVideoAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        videoPlayer.Prepare(); // Start preparing the video
        while (!videoPlayer.isPrepared) // Wait until it's ready
        {
            yield return null;
        }

        tvRenderer.material = videoMaterial; // Switch to video material
        videoPlayer.Play();
        Debug.Log("Video started after " + delay + " seconds.");
    }
}
