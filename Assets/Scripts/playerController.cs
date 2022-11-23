using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Local variables
    Rigidbody2D rb;
    bool isGrounded = false;
    Vector2 forceToAdd = new Vector2(0, 0);

    // Local variables that are editable in inspect menu
    [SerializeField] float speed = 16.0f;
    [SerializeField] float jump = 0.25f;
    [SerializeField] float grav = 1.0f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.11f;
    [SerializeField] LayerMask groundMask;


    // Awake is called on the first frame
    void Awake()
    {

    }

    // Update is called at fixed intervals
    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1, groundDist), groundMask);
        // Create force to allow for player movement
        forceToAdd.x = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;

        if (!isGrounded)
        {
            print(forceToAdd.y);
            forceToAdd.y = forceToAdd.y + (-grav * Time.fixedDeltaTime);
            if (forceToAdd.y < -grav)
            {
                forceToAdd.y = -grav;
            }

        }
        else
        {
            forceToAdd.y = 0.0f;
        }

        // If player can jump then add force to forceToAdd
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
            forceToAdd.y = jump;
        }
        // Stop player if there is no input
        if (Input.GetAxis("Horizontal") == 0)
        {
            forceToAdd.x = 0;
        }

        // Apply forces to player
        transform.position = new Vector3(transform.position.x + forceToAdd.x, transform.position.y + forceToAdd.y, 0);
    }
}
