using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    public ThirdPersonActionsAsset ui_Action_Asset;
    public bool invertedCamera = true;
    public GameObject settingsPanel;
    public GameObject buttonsObj;
    
    private static SettingsManager instance;
    
    public static SettingsManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        ui_Action_Asset = new ThirdPersonActionsAsset();
        ui_Action_Asset.UI.Cancel.started += Do_Cancel;
        ui_Action_Asset.UI.Enable();
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

    private void OnEnable()
    {
        ui_Action_Asset.UI.Cancel.started += Do_Cancel;
    }

    private void OnDisable()
    {
        ui_Action_Asset.UI.Cancel.started -= Do_Cancel;
    }
    
    private void Do_Cancel(InputAction.CallbackContext obj)
    {
        settingsPanel.SetActive(false);
        buttonsObj.SetActive(true);
    }

    
    public void ActiveInvertCamera(bool value)
    {
        invertedCamera = value;
    }
    
    public void QuitGame(bool quit = false)
    {
        if (quit)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
