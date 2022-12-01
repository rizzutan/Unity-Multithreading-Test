using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    Rigidbody2D rb;
    bool isGrounded, isCollidingRight, isCollidingLeft = false;
    float[] forceToAdd, forceToAddThread = new float[2] {0.0f, 0.0f};
    float horizontal = 0.0f;
    float vertical = 0.0f;
    bool workAround = true;
    float delta, threadDelta = 0.0f;

    // Local variables that are editable in inspect menu
    [SerializeField] float speed = 16.0f;
    [SerializeField] float jump = 0.0125f;
    [SerializeField] float grav = 0.05f;

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
        // Apply forces to player
        delta = Time.deltaTime;
        lock (forceToAddThread)
        {
            forceToAdd = forceToAddThread;
        }
        transform.position = new Vector3(transform.position.x + forceToAdd[0], transform.position.y + forceToAdd[1], 0);
    }

    void physics()
    {
        while (workAround)
        {
            threadDelta = delta;
            // Create force to allow for player movement
            forceToAddThread[0] = horizontal * speed * threadDelta;

            if ((isCollidingRight && forceToAddThread[0] > 0) || (isCollidingLeft && forceToAddThread[0] < 0))
            {
                forceToAddThread[0] = 0;
            }

            if (!isGrounded)
            {
                forceToAddThread[1] = forceToAddThread[1] + (-grav * threadDelta);
                if (forceToAddThread[1] < -grav)
                {
                    forceToAddThread[1] = -grav;
                }

            }
            else
            {
                forceToAddThread[1] = 0.0f;
            }

            // If player can jump then add force to forceToAdd
            if (isGrounded && vertical > 0)
            {
                forceToAddThread[1] = jump;
            }
            // Stop player if there is no input
            if (horizontal == 0)
            {
                forceToAddThread[0] = 0;
            }

            Thread.Sleep(1);
        }
    }

    private void OnDestroy()
    {
        workAround = false;
    }
}
