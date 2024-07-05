using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float moveSpeed = 8f;
    public float acceleration = 2f;
    public float deceleration = 2f;
    public float reverseSpeed = 4f; // Speed for reversing

    public Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        bool isReversing = Input.GetKey(KeyCode.R);

        // Check if the reverse button is held down

        // Disallow moving in two directions at once
        //if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        //{
        //    vertical = 0;
        //}
        //else
        //{
        //    horizontal = 0;
        //}

        // Determine movement input
        if (isReversing)
        {
            moveInput = new Vector2(horizontal, vertical).normalized; // Move in the opposite direction
        }
        else
        {
            moveInput = new Vector2(horizontal, vertical).normalized;
        }
    }

    private void FixedUpdate()
    {
        if (moveInput.magnitude > 0 && !Input.GetKey(KeyCode.R))
        {
            // Accelerate towards the target velocity
            float targetSpeed = moveInput.magnitude * (Input.GetKey(KeyCode.R) ? reverseSpeed : moveSpeed);
            currentVelocity = Vector2.Lerp(currentVelocity, moveInput * targetSpeed, Time.fixedDeltaTime * acceleration);
            RotateBoat();
        }
        else
        {
            // Decelerate to a stop
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }

        if (moveInput.magnitude > 0 && Input.GetKey(KeyCode.R))
        {
            // Accelerate towards the target velocity
            float targetSpeed = moveInput.magnitude * (Input.GetKey(KeyCode.R) ? reverseSpeed : moveSpeed);
            currentVelocity = Vector2.Lerp(currentVelocity, moveInput * targetSpeed, Time.fixedDeltaTime * acceleration);
            RotateBoat();
        }
        else
        {
            // Decelerate to a stop
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }

        rb.velocity = currentVelocity;
    }

    private void RotateBoat()
    {
        // Rotate the boat to face the direction of movement, but not when reversing
        if (moveInput != Vector2.zero && !Input.GetKey(KeyCode.R))
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * acceleration);
        }

        if (moveInput != Vector2.zero && Input.GetKey(KeyCode.R))
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * acceleration);
        }
    }
}
