using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private CinemachineBrain brain;
    private CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        brain = GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        freeLook.m_YAxis.m_InvertInput = SettingsManager.Instance.invertedCamera;
    }

    public void Move_To_Spot(Transform spot)
    {
        brain.enabled = false;
        transform.position = spot.position;
        transform.rotation = spot.rotation;
    }

    public void Move_To_Third_Person()
    {
        brain.enabled = true;
    }

}
