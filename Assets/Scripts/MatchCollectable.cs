using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCollectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            if (other.gameObject.GetComponent<PlayerController>().mainCharacter == true)
            {
                other.gameObject.GetComponent<PlayerController>().AddMatch();
                Destroy(gameObject);
            }
        }
    }
}
