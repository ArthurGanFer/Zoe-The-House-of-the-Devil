using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public float speechTime = 4f;
    public GameObject[] speeches;

    private void Awake()
    {

    }

    public void StartDialog()
    {
        StartCoroutine(SpeechCoroutine());
    }

    private IEnumerator SpeechCoroutine()
    {
        foreach (GameObject speech in speeches)
        {
            speech.SetActive(true);
            yield return new WaitForSeconds(speechTime);
            speech.SetActive(false);
        }
        
    }
    
}
