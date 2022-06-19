using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    public float power = 10f;
    private Rigidbody2D rb;
    public Vector2 minPower;
    public Vector2 maxPower;
    Camera cam;
    Vector2 force;
    public Vector3 startPoint;
    public Vector3 endPoint;
     public Vector3 startpos;
    public Vector3 endpos;
    public float airDrag = 50;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 0;
            rb.velocity = Vector3.zero;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 0;
        }

        if(Input.GetMouseButtonUp(0))
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 0;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            rb.AddForce(force * power , ForceMode2D.Impulse);
        }

        if(rb.velocity != Vector2.zero)
        {
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }

    
    }
}
