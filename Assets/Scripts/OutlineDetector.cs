using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Line"))
        {
            Debug.Log("Outline Detected");
            DrawCompleted drawCompleted = transform.parent.GetComponent<DrawCompleted>();
            if (drawCompleted != null)
            {
                drawCompleted.outlineDetected = true;
            }
            else
            {
                Debug.LogWarning("DrawCompleted component not found on the GameObject.");
            }
        }
    }
}
