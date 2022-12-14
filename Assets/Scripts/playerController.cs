using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    private Rigidbody2D rb;
    private bool isGrounded, isCollidingRight, isCollidingLeft = false;
    private float[] forceToAdd = new float[2] {0.0f, 0.0f};
    private float horizontal, vertical = 0.0f;
    private bool workAround = true;
    private float delta, threadDelta = 0.0f;
    private int fixedUpdate;

    // Local variables that are editable in inspect menu
    [SerializeField] private float speed = 16.0f;
    [SerializeField] private float jump = 0.0125f;
    [SerializeField] private float grav = 0.05f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform rightCheck;
    [SerializeField] private Transform leftCheck;
    [SerializeField] private float dist = 0.055f;
    [SerializeField] private LayerMask groundMask;


    // Awake is called on the first frame
    void Awake()
    {
        fixedUpdate = (int)((Time.fixedDeltaTime * 1000) + 1);
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
        lock (forceToAdd)
        {
            transform.position = new Vector3(transform.position.x + forceToAdd[0], transform.position.y + forceToAdd[1], 0);
        }
    }

    void physics()
    {
        while (workAround)
        {
            threadDelta = delta;
            // Create force to allow for player movement
            forceToAdd[0] = horizontal * speed * threadDelta;

            if ((isCollidingRight && forceToAdd[0] > 0) || (isCollidingLeft && forceToAdd[0] < 0))
            {
                forceToAdd[0] = 0;
            }

            if (!isGrounded)
            {
                forceToAdd[1] = forceToAdd[1] + (-grav * threadDelta);
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

            Thread.Sleep(fixedUpdate);
        }
    }

    private void OnDestroy()
    {
        workAround = false;
    }
}
