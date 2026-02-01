using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Dot"))
        {
            Debug.Log("Dot Detected");
            obj.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
