using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float thrust = 10f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.5f;
    public float decelerationDistance = 2f;
    public float maxSpeed = 5f;
    public float baseDrag = 1f;
    public float maxDrag = 10f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMovingToTarget = false;
    [SerializeField] bool ManualControlMode = true; // true for manual, false for automatic

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = baseDrag;
    }

    void Update()
    {
        if (ManualControlMode)
        {
            ManualMovement();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = new Vector2(mousePos.x, mousePos.y);
                isMovingToTarget = true;
            }

            if (isMovingToTarget)
            {
                MoveToTarget();
            }
        }
    }

    void ManualMovement()
    {
        float move = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");

        rb.AddForce(transform.up * move * thrust);

        float rotationAmount = rotate * rotationSpeed;
        rb.rotation -= rotationAmount; // Rotate based on input
    }

    void RotateTowardsTarget()
    {
        Vector2 direction = (targetPosition - rb.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angleDifference = Mathf.DeltaAngle(rb.rotation, angle);
        float rotationAmount = Mathf.Clamp(angleDifference, -rotationSpeed, rotationSpeed);
        rb.rotation += rotationAmount;
    }

    void MoveToTarget()
    {
        RotateTowardsTarget();
        Vector2 direction = (targetPosition - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, targetPosition);
        float stopThreshold = stopDistance + Mathf.Max(rb.velocity.magnitude * Time.fixedDeltaTime, 0.1f); // Consider current velocity for stopping

        if (distance > stopThreshold)
        {
            rb.drag = Mathf.Lerp(baseDrag, maxDrag, 1 - (distance / decelerationDistance));
            rb.AddForce(direction * thrust);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.1f);
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero;
                rb.drag = baseDrag; // Reset drag to base value
                isMovingToTarget = false; // Reset the flag since the ship has stopped at the target
            }
        }
    }
}