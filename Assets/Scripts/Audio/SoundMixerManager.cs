using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audio_Mixer;

    public void SetMasterVolume(float volume)
    {
        audio_Mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audio_Mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }
    
    public void SetSoundFXVolume(float volume)
    {
        audio_Mixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20);
    }
}
