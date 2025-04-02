using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public string sceneToLoad;
    VideoPlayer videoPlayer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void EndReached(VideoPlayer vp)
    {
        GameManager.Instance.LoadLevel(sceneToLoad);
    }
    
}
