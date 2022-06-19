using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 startPoint, endPoint, appliedForce, forceVector;
    bool isMoving;
    [SerializeField] float airDrag, maxPower, minVelocity, power;

    void Start()
    {
        isMoving = false;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (rb.velocity.magnitude > minVelocity)
        {
            isMoving = true;
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }
        else
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }

        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                forceVector = startPoint - endPoint;
                appliedForce = forceVector.normalized * Mathf.Clamp(forceVector.magnitude, -maxPower, maxPower);
                rb.AddForce(appliedForce * power, ForceMode2D.Impulse);
                Debug.Log("Force Vector: " + forceVector + ", Applied Force: " + appliedForce);
            }
        }
    }
}