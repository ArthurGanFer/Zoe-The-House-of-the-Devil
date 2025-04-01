using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cutscene : MonoBehaviour
{
    public CameraController cameraController;
    public UnityEvent onStart;
    public float duration;
    public UnityEvent onFinish;
    
    public void PlayCutscene()
    {
        onStart?.Invoke();
        cameraController.Move_To_Spot(transform);
        StartCoroutine(CutsceneCoroutine());
    }

    public IEnumerator CutsceneCoroutine()
    {
        yield return new WaitForSeconds(duration);
        onFinish?.Invoke();
        cameraController.Move_To_Third_Person();
    }

}
