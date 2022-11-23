using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    Rigidbody2D rb;
    bool isGrounded, isCollidingRight, isCollidingLeft = false;
    float[] forceToAdd = new float[2] { 0.0f, 0.0f};

    // Local variables that are editable in inspect menu
    [SerializeField] float speed = 16.0f;
    [SerializeField] float jump = 0.25f;
    [SerializeField] float grav = 1.0f;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform rightCheck;
    [SerializeField] Transform leftCheck;
    [SerializeField] float dist = 0.055f;
    [SerializeField] LayerMask groundMask;


    // Awake is called on the first frame
    void Awake()
    {

    }

    // Update is called at fixed intervals
    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1, dist), groundMask);
        //Check if the player is colliding with wall on either side
        isCollidingRight = Physics2D.OverlapBox(rightCheck.position, new Vector2(dist, 0.9f), groundMask);
        isCollidingLeft = Physics2D.OverlapBox(leftCheck.position, new Vector2(dist, 0.9f), groundMask);
        // Create force to allow for player movement
        forceToAdd[0] = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;

        if ((isCollidingRight && forceToAdd[0] > 0) || (isCollidingLeft && forceToAdd[0] < 0))
        {
            forceToAdd[0] = 0;
        }

        if (!isGrounded)
        {
            print(forceToAdd[1]);
            forceToAdd[1] = forceToAdd[1] + (-grav * Time.fixedDeltaTime);
            if (forceToAdd[1] < -grav)
            {
                forceToAdd[1] = -grav;
            }

        }
        else
        {
            forceToAdd[1] = 0.0f;
        }

        // If player can jump then add force to forceToAdd
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
            forceToAdd[1] = jump;
        }
        // Stop player if there is no input
        if (Input.GetAxis("Horizontal") == 0)
        {
            forceToAdd[0] = 0;
        }

        // Apply forces to player
        transform.position = new Vector3(transform.position.x + forceToAdd[0], transform.position.y + forceToAdd[1], 0);
    }
}
