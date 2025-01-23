using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public Transform[] hookLocs;
    public LayerMask hooks;
    public bool foundTarget;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            FindTarget();
        }
    }

    public void FindTarget()
    {

        Collider[] hooksCol = Physics.OverlapSphere(transform.position, 5000, hooks);
        for (int i = 0; i < hooksCol.Length; i++) {
        
            hookLocs[i] = hooksCol[i].transform;
        }
        Debug.Log(hooksCol);

        if (hookLocs.Length > 0)
        {
            foundTarget = true;
        }
        
    }
}
