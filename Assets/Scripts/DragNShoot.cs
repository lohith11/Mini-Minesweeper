using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    [SerializeField]
    private float power = 10f;
    private Rigidbody2D rb;
    public Vector2 minPower;
    public Vector2 maxPower;
    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;
    public float airDrag = 50;

     TrajectoryLine tl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 15;
        }

        if(Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToViewportPoint(Input.mousePosition);
            startPoint.z = 15;
            tl.Renderline(startPoint, currentPoint);
        }

        if(Input.GetMouseButtonUp(0))
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            Debug.Log(force);
            rb.AddForce(force * power , ForceMode2D.Impulse);
            tl.endLine();
        }

        if(rb.velocity != Vector2.zero)
        {
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }
    }
}
