using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHelper : MonoBehaviour
{

    public void ChangeMusic(AudioClip nextMusic)
    {
        MusicManager.Instance.ChangeMusic(nextMusic);
    }

    public void PauseMusic()
    {
        MusicManager.Instance.PauseMusic();
    }
}
