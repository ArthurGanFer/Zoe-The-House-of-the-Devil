using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject fadeInPrefab;
    public GameObject fadeOutPrefab;
    
    private CinemachineBrain brain;
    private CinemachineFreeLook freeLook;
    private SettingsManager settingsManager;
    // Start is called before the first frame update
    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        brain = GetComponent<CinemachineBrain>();
        settingsManager = FindFirstObjectByType<SettingsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsManager != null)
        {
            freeLook.m_YAxis.m_InvertInput = SettingsManager.Instance.invertedCamera;
        }
    }

    public void Move_To_Spot(Transform spot)
    {
        Instantiate(fadeInPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        brain.enabled = false;
        transform.position = spot.position;
        transform.rotation = spot.rotation;
    }

    public void Move_To_Third_Person()
    {
        Instantiate(fadeInPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        brain.enabled = true;
    }

}
