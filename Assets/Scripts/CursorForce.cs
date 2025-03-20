using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorForce : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
