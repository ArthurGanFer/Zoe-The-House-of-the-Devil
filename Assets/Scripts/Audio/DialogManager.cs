using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    [SerializeField]
    private Dialog current_Dialog;
    [SerializeField]
    private Dialog[] dialogs;
    private int dialog_Index = 0;
    private int speech_Index = 0;

    [SerializeField] private AudioSource dialog_Sound_Object;
    
    public static DialogManager Instance
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

    public void StartNewDialog(int dialogIndex)
    {
        if (dialogIndex >= dialogs.Length)
        {
            dialogIndex = 0;
            Debug.LogWarning("Dialog Index out of range.");
        }
        
        current_Dialog = dialogs[dialogIndex];
        speech_Index = 0;
    }

    public void PlaySpeechClip(int speechIndex)
    {
        if (current_Dialog == null)
            StartNewDialog(0);
        /*
        AudioSource audio_source = Instantiate(dialog_Sound_Object, transform.position, Quaternion.identity);
        audio_source.clip = current_Dialog.Speeches[speechIndex].Audio_Clip;
        
        audio_source.Play();
        
        float clip_length = audio_source.clip.length;
        */
        Debug.Log(current_Dialog.Speeches[speechIndex].Speech_Text);
        
        //Destroy(audio_source.gameObject, clip_length);
    }

    public void PlayCurrentSpeech()
    {
        PlaySpeechClip(speech_Index);
        speech_Index++;
        
        if (speech_Index >= dialogs.Length)
            StartNewDialog(++dialog_Index);
    }
    
}
