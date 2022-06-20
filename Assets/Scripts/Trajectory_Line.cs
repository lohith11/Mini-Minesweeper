using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory_Line : MonoBehaviour
{
    Vector2 startPos, endPos, mouseStart, mouseEnd;
    Camera cam;
    LineRenderer lineRenderer;

    DragNShoot ball;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        ball = FindObjectOfType<DragNShoot>();
        cam = Camera.main;
    }
    void Update()
    {
        if(ball.IsMoving)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.enabled = true;
            startPos = ball.transform.position;
            mouseStart = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            lineRenderer.SetPosition(0, startPos);
            mouseEnd = cam.ScreenToWorldPoint(Input.mousePosition);

            endPos = (mouseEnd - mouseStart) + startPos;
            float capLength = Mathf.Clamp(Vector2.Distance(startPos, endPos), 0, 3);
            endPos = startPos + ( - (mouseEnd - mouseStart).normalized * capLength);
            lineRenderer.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
        }
    }
}
