using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    Rigidbody2D rb;
    bool isGrounded = false;

    // Local variables that are editable in inspect menu
    [SerializeField] float speed = 16.0f;
    [SerializeField] float jump = 500.0f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.11f;
    [SerializeField] LayerMask groundMask;


    // Awake is called on the first frame
    void Awake()
    {
        // Get RigidBody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called at fixed intervals
    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDist, groundMask);
        // Create force to allow for player movement
        Vector2 forceToAdd = new Vector2(Input.GetAxis("Horizontal") * speed, 0);
        // If player can jump then add force to forceToAdd
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
            forceToAdd.y = jump;
        }
        // If player speed is greater than speed then add no velocity to player
        if (rb.velocity.x > speed || rb.velocity.x < -speed)
        {
            // Set player velocity back to the max speed
            forceToAdd.x = speed - rb.velocity.x;
        }
        // Stop player if there is no input
        if (Input.GetAxis("Horizontal") == 0)
        {
            forceToAdd.x = -1.5f * rb.velocity.x;
        }
        // Apply forces to player
        rb.AddForce(forceToAdd);
    }
}
