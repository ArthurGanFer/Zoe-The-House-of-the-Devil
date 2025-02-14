using System.Collections;
using UnityEngine;

public class ObjectBreak : MonoBehaviour
{
    [SerializeField] private GameObject wholeObject;
    [SerializeField] private GameObject[] brokenParts;
    public float destroyAfterSeconds = 5f;

    public void DestroyPart()
    {
        if (wholeObject != null)
        {
            wholeObject.SetActive(false);
        }

        if (wholeObject == null)
        {
            foreach (GameObject part in brokenParts)
            {
                part.SetActive(true);
            }
        }


        //if (brokenParts != null && brokenParts.Length > 0)
        //{
        //    foreach (GameObject part in brokenParts)
        //    {
        //        if (part != null)
        //        {
        //            part.SetActive(true);
        //        }
        //    }

        //    StartCoroutine(DestroyShardsAfterDelay());
        //}
    }

    private IEnumerator DestroyShardsAfterDelay()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);

        if (brokenParts != null && brokenParts.Length > 0)
        {
            foreach (GameObject part in brokenParts)
            {
                if (part != null)
                {
                    Destroy(part);
                }
            }
        }
    }
}
