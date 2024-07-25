using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public float moveSpeed = 8f;
    public float acceleration = 3f;
    public float deceleration = 3f;
    public float reverseSpeed = 4f; // Speed for reversing

    public Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;

    private bool oars, motor;

    // Animator component
    private Animator animator;

    private float previousRotation;
    private const float RotationThreshold = 0.5f; // Adjust threshold as needed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
        previousRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        bool isReversing = Input.GetKey(KeyCode.R);

        // Determine movement input
        moveInput = new Vector2(horizontal, vertical).normalized;

        // Update Animator parameters for rowing direction and straight movement
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Debug.Log($"MoveInput: {moveInput}, Horizontal: {horizontal}, Vertical: {vertical}");
    }

    private void FixedUpdate()
    {
        if (moveInput.magnitude > 0)
        {
            // Accelerate towards the target velocity
            float targetSpeed = moveInput.magnitude * (Input.GetKey(KeyCode.R) ? reverseSpeed : moveSpeed);
            currentVelocity = Vector2.Lerp(currentVelocity, moveInput * targetSpeed, Time.fixedDeltaTime * acceleration);
        }
        else
        {
            // Decelerate to a stop
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }

        rb.velocity = currentVelocity;

        // Always call RotateBoat to ensure animation state updates
        RotateBoat();
    }

    private void RotateBoat()
    {
        // Calculate current rotation
        float currentRotation = transform.rotation.eulerAngles.z;

        // Calculate rotation difference
        float rotationDifference = Mathf.DeltaAngle(previousRotation, currentRotation);

        // Determine rotation direction
        bool clockwise = rotationDifference < 0;

        // Update previous rotation for the next frame
        previousRotation = currentRotation;

        // Check if the boat is moving straight and not rotating
        bool isMovingStraight = (Mathf.Abs(horizontal) >= .5f || Mathf.Abs(vertical) >= .5f) && Mathf.Abs(rotationDifference) <= RotationThreshold;

        Debug.Log($"RotationDifference: {rotationDifference}, IsMovingStraight: {isMovingStraight}");

        // Explicit check for moveInput != Vector2.zero
        if (moveInput != Vector2.zero)
        {
            Debug.Log("Boat is moving.");
            if (isMovingStraight)
            {
                animator.SetBool("RowBoth", true);
                animator.SetBool("RowRight", false);
                animator.SetBool("RowLeft", false);
                Debug.Log("STRAIGHT");
            }
            else
            {
                animator.SetBool("RowRight", clockwise);
                animator.SetBool("RowLeft", !clockwise);
                animator.SetBool("RowBoth", false);
                Debug.Log("TURNING");
            }
        }
        else
        {
            // This block handles the case when moveInput is (0, 0)
            Debug.Log("Boat is not moving.");
            animator.SetBool("RowRight", false);
            animator.SetBool("RowLeft", false);
            animator.SetBool("RowBoth", false);
            Debug.Log("Rowing Both: false");
            Debug.Log("RowRight: false");
            Debug.Log("RowLeft: false");
        }

        // Rotate the boat to face the direction of movement, but always rotated 180 degrees                                        
        if (moveInput != Vector2.zero)// && !Input.GetKey(KeyCode.R))                                                               // AS OF NOW, HOLDING 'R' JUST LOWERS THE SPEED BY HALF BUT DOES NOT REVERSE
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f + 180f; // Add 180 degrees
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * acceleration);
        }

        //if (moveInput != Vector2.zero && Input.GetKey(KeyCode.R))                                                                 // BRING THIS BACK WHEN YOU HAVE MOTOR AVAILABLE
        //{
        //    float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg + 90f + 180f; // Add 180 degrees
        //    Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));
        //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * acceleration);
        //}
    }
}
