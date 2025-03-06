using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public bool changeScene = false;
    public string sceneToLoad;
    
    [SerializeField]
    private UnityEvent event_Action;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            event_Action?.Invoke();
            if (changeScene == true)
            {
                GameManager.Instance.LoadLevel(sceneToLoad);
            }
        }
    }
    
    

}
