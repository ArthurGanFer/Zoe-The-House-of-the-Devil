using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Speech
{
    public AudioClip Audio_Clip;
    [TextArea(5,10)]
    public string Speech_Text;
}
