using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Grappling grappleScript;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        grappleScript = GetComponent<Grappling>();
    }

    // Update is called once per frame
    void Update()
    {
        //grappleScript.FindTarget();
        // target = grappleScript.hookLocs[0];

        // transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);

        //Debug.Log(target.position.ToString());

        if (grappleScript.foundTarget)
        {
            target = grappleScript.hookLocs[0].transform;
            transform.position  = Vector3.Lerp(transform.position, target.position, 500 * Time.deltaTime);
        }
            
       
    }
}
