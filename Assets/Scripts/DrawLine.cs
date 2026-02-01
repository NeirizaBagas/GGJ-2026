using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    public GameObject linePrefab; // Prefab for the line
    GameObject currentLineObj; // Current line being drawn
    LineRenderer currentLine;
    EdgeCollider2D edgeCollider;

    public float linePositionZ;
    Vector3 mousePosition;
    Vector3 mousePositionXY;
    bool isDrawing = true;
    bool canDraw = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        DrawCompleted.OnDrawComplete += () => { canDraw = false; };
    }

    private void OnDisable()
    {
        DrawCompleted.OnDrawComplete -= () => { canDraw = false; };
    }

    void OnMouseDown()
    {
        if (!canDraw) return;
        CreateLine();
        Debug.Log("Mouse Down");
    }

    private void OnMouseDrag()
    {
        UpdateLine();
    }

    private void OnMouseExit()
    {
        isDrawing = false;
        //EndDraw();
    }

    private void OnMouseUp()
    {
        EndDraw();
    }

    private void CreateLine()
    {
        isDrawing = true;
        currentLineObj = Instantiate(linePrefab);
        currentLine = currentLineObj.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        currentLine.SetPosition(0, GetMousePosition());
        currentLine.SetPosition(1, GetMousePosition());
        edgeCollider.points = EdgePoint();
    }

    private void UpdateLine()
    {
        if (currentLine.GetPosition(currentLine.positionCount - 1) != GetMousePosition() && isDrawing)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, GetMousePosition());
            edgeCollider.points = EdgePoint();
        }
    }

    private void EndDraw()
    {
        if (currentLine != null)
        {
            currentLineObj.transform.parent = GameObject.Find("Lines").transform; // Parent the line to a "Lines" GameObjec
        }
        else
        {
            return;
        }
    }

    Vector3 GetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositionXY = new Vector3(mousePosition.x, mousePosition.y, linePositionZ);
        return mousePositionXY;
    }

    Vector2[] EdgePoint()
    {
        Vector2[] points = new Vector2[currentLine.positionCount];
        for (int i = 0; i < currentLine.positionCount; i++)
        {
            points[i] = currentLine.GetPosition(i);
        }
        return points;
    }

    public bool CanDraw
    {
        get { return canDraw; }
        set { canDraw = value; }
    }
}
