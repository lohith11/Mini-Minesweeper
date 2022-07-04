using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 startPoint, endPoint, appliedForce, forceVector;
    public bool IsMoving;
    [SerializeField] float airDrag, maxPower, minVelocity, power, minBreakVelocity;
    [SerializeField] GameObject collisionParticles, trailParticles;

    void Start()
    {
        //trailParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        IsMoving = false;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (rb.velocity.magnitude > minVelocity)
        {
            IsMoving = true;
            //trailParticles.SetActive(true);
            rb.AddForce(-rb.velocity.normalized * airDrag);
        }
        else
        {
            IsMoving = false;
            //trailParticles.SetActive(false);
            rb.velocity = Vector2.zero;
            // if (!GameManager.gameManagerInstance.gameStarted)
            //     GameManager.gameManagerInstance.gameStarted = true;
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
                //Debug.Log("Force Vector: " + forceVector + ", Applied Force: " + appliedForce);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CellProperties cellProps;
        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.gameObject.name == "Tile" && rb.velocity.magnitude > minBreakVelocity)
        {
            Instantiate(collisionParticles, other.transform.position + (transform.position - other.transform.position).normalized * (Vector3.Distance(transform.position, other.transform.position) / 2f), Quaternion.identity);
            AudioManager.audioManagerInstance.Play("Bump");
            cellProps = other.transform.parent.GetComponent<CellProperties>();
            InputManager.inputManagerInstance.ClickedOnTile(cellProps);
        }
    }
}