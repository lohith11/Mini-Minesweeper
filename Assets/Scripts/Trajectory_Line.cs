using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory_Line : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 mousePos;
    Vector3 mouseDir;
    Camera cam;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = mousePos - gameObject.transform.position;
        mouseDir.z= 0;
        mouseDir = mouseDir.normalized;

        if(Input.GetMouseButtonDown(0))
        {
            lr.enabled = true;
        }
        if(Input.GetMouseButton(0))
        {
            startPos = gameObject.transform.position;
            startPos.z = 0;
            lr.SetPosition(0 , startPos);
            endPos = mousePos; 
            endPos.z = 0;
            float capLength = Mathf.Clamp(Vector2.Distance(startPos,endPos) , 0 , 3);
            endPos = startPos + (  mouseDir * capLength);
            lr.SetPosition(1, endPos);
        }

        if(Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
        }
    }
}
