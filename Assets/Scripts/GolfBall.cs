using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    [SerializeField]
    private float force = 5; //! The force is variable with this one
    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (Vector2) (mousePosition - transform.position);

        if(Input.GetMouseButtonDown(0))
        {
            rb.AddForce(direction * force);
        }
    }
}
