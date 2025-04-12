using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public GameObject yesButton;
    
    void OnEnable()
    {
        SettingsManager.Instance.MakeButtonSelected(yesButton);
    }

}
