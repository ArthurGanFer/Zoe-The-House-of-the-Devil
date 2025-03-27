using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour
{

    public void PlaySoundFromPosition(AudioClip sound)
    {
        SoundFXManager.instance.transform.position = transform.position;
        SoundFXManager.instance.PlaySoundFX(sound);
    }
    
}
