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
        audio_Mixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audio_Mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVoiceVolume(float volume)
    {
        audio_Mixer.SetFloat("voiceVolume", Mathf.Log10(volume) * 20);
    }
}
