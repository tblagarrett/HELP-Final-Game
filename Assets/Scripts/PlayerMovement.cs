using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float thrust = 5f;
    public float stopThreshold = 0.1f;

    private Rigidbody rb;
    public float distanceToGround;
    private LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

        if (!Physics.Raycast(transform.position, Vector3.down, distanceToGround))
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(verticalInput) > stopThreshold && Mathf.Abs(horizontalInput) > stopThreshold)
        {
            Vector2 vector2 = new Vector2(horizontalInput, verticalInput).normalized;
            rb.velocity = new Vector3(thrust * vector2.x, rb.velocity.y, thrust * vector2.y);
            return;
        }

        // Vertical Movement
        if (Mathf.Abs(verticalInput) < stopThreshold)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
        } 
        else if (Mathf.Abs(horizontalInput) < stopThreshold)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, thrust * verticalInput);
        }

        // Horizontal Movement
        if (Mathf.Abs(horizontalInput) < stopThreshold)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        }
        else if (Mathf.Abs(verticalInput) < stopThreshold)
        {
            rb.velocity = new Vector3(thrust * horizontalInput, rb.velocity.y, rb.velocity.z);
        }
    }

    // Teleport the player to the Current Room's spawn
    public void TeleportToSpawn()
    {
        levelManager = LevelManager.Instance;
        GetComponent<Transform>().position = levelManager.CurrentRoom().GetComponent<Room>().GetSpawnLocation();
    }
}
