using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFX_Object;

    public static SoundFXManager Instance
    {
        get { return instance; }
    }

    private void Awake()
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

    public void PlaySoundFX(AudioClip audioClip)
    {
        AudioSource audio_source = Instantiate(soundFX_Object, transform.position, Quaternion.identity);
        
        audio_source.clip = audioClip;
        
        audio_source.Play();
        
        float clip_length = audio_source.clip.length;
        
        Destroy(audio_source.gameObject, clip_length);
    }
}
