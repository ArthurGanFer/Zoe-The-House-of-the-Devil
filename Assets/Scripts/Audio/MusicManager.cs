using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private float currentVolume;
    private float fadeChangeDuration = 2f;
    
    private static MusicManager instance;

    public static MusicManager Instance
    {
        get { return instance; }
    }
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentVolume = audioSource.volume;
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

    public void ChangeMusic(AudioClip nextMusic)
    {
        StartCoroutine(FadeMusic(nextMusic));
    }
    
    public IEnumerator FadeMusic(AudioClip clip)
    {
        for (float t = 0f; t < fadeChangeDuration; t += Time.deltaTime) {
            audioSource.volume = Mathf.Lerp(currentVolume, 0, t / fadeChangeDuration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.clip = clip;
        audioSource.Play();
        for (float t = 0f; t < fadeChangeDuration; t += Time.deltaTime) {
            audioSource.volume = Mathf.Lerp(0, currentVolume, t / fadeChangeDuration);
            yield return null;
        }
        audioSource.volume = currentVolume;
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }
    
}
