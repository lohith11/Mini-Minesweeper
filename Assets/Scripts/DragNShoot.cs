using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 startPoint, endPoint, appliedForce, forceVector;
    public bool IsMoving { get; private set; }
    [SerializeField] float airDrag, maxPower, minVelocity, power;

    void Start()
    {
        IsMoving = false;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (!GameManager.gameManagerInstance.gameStarted)
            return;

        if (rb.velocity.magnitude > minVelocity)
        {
            IsMoving = true;
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }
        else
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
        }

        if (!IsMoving)
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