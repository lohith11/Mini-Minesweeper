using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 startPoint, endPoint, appliedForce, forceVector;
    public bool IsMoving { get; private set; }
    [SerializeField] float airDrag, maxPower, minVelocity, power, minBreakVelocity;

    void Start()
    {
        IsMoving = false;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (rb.velocity.magnitude > minVelocity)
        {
            IsMoving = true;
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }
        else
        {
            rb.velocity = Vector2.zero;
            IsMoving = false;
            if (!GameManager.gameManagerInstance.gameStarted)
                GameManager.gameManagerInstance.gameStarted = true;
        }

        if (!GameManager.gameManagerInstance.gameStarted)
            return;


        if (!IsMoving && GameManager.gameManagerInstance.gameStarted)
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

    void OnCollisionEnter2D(Collision2D other)
    {
        CellProperties cellProps;
        Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject.name == "Tile" && rb.velocity.magnitude > minBreakVelocity)
        {
            Debug.Log("Collided!");
            cellProps = other.transform.parent.GetComponent<CellProperties>();
            InputManager.inputManagerInstance.ClickedOnTile(cellProps);
        }
    }
}