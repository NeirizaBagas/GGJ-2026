using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DrawingTracer : MonoBehaviour
{
    [Header("Drawing Setup")]
    public Camera cam;
    public GameObject brushPrefab;
    public float minDistanceBetweenPoints = 0.1f;

    [Header("Invisible Path Logic")]
    public Transform[] targetPath;
    public float tolerance = 0.5f;
    public float penaltyAmount = 20f;
    public float finishRadius = 0.5f; 

    [Header("Status")]
    public float currentScore = 100f;
    public bool isFinished = false;

    [Header("Events")]
    public static Action levelComplete;

    private LineRenderer currentLineRenderer;
    private Vector2 lastPos;
    private Vector2[] targetPathPoints;
    [SerializeField] private List<GameObject> strokes;
    [SerializeField] private GameObject pathRef;

    public static Action _OnLevelComplete;

    void Start()
    {
        if (targetPath != null && targetPath.Length > 0)
        {
            targetPathPoints = new Vector2[targetPath.Length];
            for (int i = 0; i < targetPath.Length; i++)
            {
                targetPathPoints[i] = targetPath[i].position;
            }
        }
    }

    void Update()
    {
        if (isFinished) return; // Stop logic once complete

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }

        if (Input.GetKey(KeyCode.Mouse0) && currentLineRenderer != null)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(mousePos, lastPos) > minDistanceBetweenPoints)
            {
                AddPoint(mousePos);
                lastPos = mousePos;
                ValidatePoint(mousePos);
                CheckForFinish(mousePos); // Added finish check
            }
        }
    }

    void CheckForFinish(Vector2 p)
    {
        if (targetPathPoints == null || targetPathPoints.Length == 0) return;

        // Get the very last point in the array
        Vector2 finalPoint = targetPathPoints[targetPathPoints.Length - 1];

        // Check if player is close enough to the end
        if (Vector2.Distance(p, finalPoint) < finishRadius)
        {
            isFinished = true;
            _OnLevelComplete?.Invoke();
            //StartCoroutine(OnLevelComplete());
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        //for(int i = 0; i < targetPath.Length; i++)
        //{
        //    targetPath[i].gameObject.SetActive(false);
        //}

        foreach (Transform go in targetPath)
        {
            go.gameObject.SetActive(false);
        }

        foreach (GameObject go in strokes)
        {
            go.gameObject.SetActive(false);
        }

        pathRef.SetActive(false);
    }

    //IEnumerator OnLevelComplete()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    //for(int i = 0; i < targetPath.Length; i++)
    //    //{
    //    //    targetPath[i].gameObject.SetActive(false);
    //    //}

    //    foreach (Transform go in targetPath)
    //    {
    //        go.gameObject.SetActive(false);
    //    }

    //    foreach (GameObject go in strokes)
    //    {
    //        go.gameObject.SetActive(false);
    //    }

    //    pathRef.SetActive(false);
    //}

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brushPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        currentLineRenderer.positionCount = 0;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        AddPoint(mousePos);
        lastPos = mousePos;

        strokes.Add(brushInstance);
    }

    void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, pointPos);
    }

    void ValidatePoint(Vector2 p)
    {
        if (targetPathPoints == null || targetPathPoints.Length < 2) return;
        float minDistance = float.MaxValue;
        for (int i = 0; i < targetPathPoints.Length - 1; i++)
        {
            float d = GetDistanceToSegment(targetPathPoints[i], targetPathPoints[i + 1], p);
            if (d < minDistance) minDistance = d;
        }

        //if (minDistance > tolerance)
        //{
        //    currentScore -= penaltyAmount * Time.deltaTime;
        //    currentLineRenderer.startColor = Color.red;
        //    currentLineRenderer.endColor = Color.red;
        //}
        //else
        //{
        //    currentLineRenderer.startColor = Color.white;
        //    currentLineRenderer.endColor = Color.white;
        //}
    }

    float GetDistanceToSegment(Vector2 a, Vector2 b, Vector2 p)
    {
        float l2 = Vector2.SqrMagnitude(a - b);
        if (l2 == 0.0f) return Vector2.Distance(p, a);
        float t = Mathf.Clamp01(Vector2.Dot(p - a, b - a) / l2);
        Vector2 projection = a + t * (b - a);
        return Vector2.Distance(p, projection);
    }
}