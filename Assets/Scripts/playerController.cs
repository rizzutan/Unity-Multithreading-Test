using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    Rigidbody2D rb;
    bool isGrounded, isCollidingRight, isCollidingLeft = false;
    float[] forceToAdd = new float[2] {0.0f, 0.0f};
    float[] forceToAddMain = new float[2] { 0.0f, 0.0f };
    float horizontal = 0.0f;
    float vertical = 0.0f;
    bool workAround = true;

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
        Thread thread = new Thread(physics);
        thread.Start();
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1, dist), groundMask);
        //Check if the player is colliding with wall on either side
        isCollidingRight = Physics2D.OverlapBox(rightCheck.position, new Vector2(dist, 0.9f), groundMask);
        isCollidingLeft = Physics2D.OverlapBox(leftCheck.position, new Vector2(dist, 0.9f), groundMask);
        // Grab player input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Jump");
    }

    void Update()
    {
        // Grab local copy of forceToAdd
        lock (forceToAdd)
        {
            forceToAddMain = forceToAdd;
        }

        // Apply forces to player
        transform.position = new Vector3(transform.position.x + forceToAddMain[0], transform.position.y + forceToAddMain[1], 0);
    }

    void physics()
    {
        while (workAround)
        {
            // Create force to allow for player movement
            forceToAdd[0] = horizontal * speed * (1/1000);

            if ((isCollidingRight && forceToAdd[0] > 0) || (isCollidingLeft && forceToAdd[0] < 0))
            {
                forceToAdd[0] = 0;
            }

            if (!isGrounded)
            {
                forceToAdd[1] = forceToAdd[1] + (-grav * (1 / 1000));
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
            if (isGrounded && vertical > 0)
            {
                forceToAdd[1] = jump;
            }
            // Stop player if there is no input
            if (horizontal == 0)
            {
                forceToAdd[0] = 0;
            }

            Thread.Sleep(1);
        }
    }

    private void OnDestroy()
    {
        workAround = false;
    }
}
