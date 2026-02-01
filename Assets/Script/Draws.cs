using Unity.VisualScripting;
using UnityEngine;

public class Draws : MonoBehaviour
{
    public Camera cam;
    public GameObject brush;

    LineRenderer currentLineRenderer;

    Vector2 lastpos;

    private void Update()
    {
        Draw();
    }

    void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != lastpos)
            {
                AddPoint(mousePos); 
                lastpos = mousePos; 
            }
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int postionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(postionIndex, pointPos);
    }
}
