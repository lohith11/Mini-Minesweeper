using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 startPoint, endPoint, appliedForce, forceVector;
    public bool IsMoving;
    bool aimingStarted = false;
    [SerializeField] float airDrag, maxPower, minVelocity, power, minBreakVelocity;
    [SerializeField] GameObject collisionParticlesTile, trailParticles, collisionParticlesWall;

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
            IsMoving = false;
            rb.velocity = Vector2.zero;
        }

        if (!GameManager.gameManagerInstance.gameStarted || GameManager.gameManagerInstance.gameEnded || IsMoving)
            return;


        if (!IsMoving && GameManager.gameManagerInstance.gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                aimingStarted = true;
            }

            if (Input.GetMouseButtonUp(0) && aimingStarted)
            {
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                forceVector = startPoint - endPoint;
                appliedForce = forceVector.normalized * Mathf.Clamp(forceVector.magnitude, -maxPower, maxPower);
                rb.AddForce(appliedForce * power, ForceMode2D.Impulse);
                AudioManager.audioManagerInstance.Play("GolfClub");
                aimingStarted = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CellProperties cellProps;
        Vector3 particlePosition = other.transform.position + (transform.position - other.transform.position).normalized * (Vector3.Distance(transform.position, other.transform.position) / 2f);

        if (other.gameObject.name == "Tile" && rb.velocity.magnitude > minBreakVelocity)
        {
            Instantiate(collisionParticlesTile, particlePosition, Quaternion.identity);
            cellProps = other.transform.parent.GetComponent<CellProperties>();
            InputManager.inputManagerInstance.ClickedOnTile(cellProps);
        }
        else if (other.transform.parent.gameObject.tag == "Wall")
            Instantiate(collisionParticlesWall, particlePosition, Quaternion.identity);

        AudioManager.audioManagerInstance.Play("Bump");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "End")
        {
            Debug.Log("Flag touched");
            GameManager.gameManagerInstance.FlagTouched();
        }
    }
}