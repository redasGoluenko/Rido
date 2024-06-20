using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    public bool Available = false;
    public bool PlayerColliding = false;

    private int pivotContactCount = 0; // Counter for "Pivot" collisions
    private int playerContactCount = 0; // Counter for "Player" collisions

    public GameObject tokenPrefab;
    void Start()
    {
        // Optional: Initialize debug states or any required setup
    }

    // Called when this collider/rigidbody has begun touching another rigidbody/collider.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pivot"))
        {
            pivotContactCount++;
            if (pivotContactCount == 1) // Only set Available to true if it's the first contact
            {
                Available = true;
                //Debug.Log("Bottom Available");
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerContactCount++;
            if (playerContactCount == 1) // Only set PlayerColliding to true if it's the first contact
            {
                PlayerColliding = true;
                //Debug.Log("Player Colliding");
            }
        }
    }

    // Called when this collider/rigidbody has stopped touching another rigidbody/collider.
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pivot"))
        {
            pivotContactCount--;
            if (pivotContactCount <= 0) // Only set Available to false if no "Pivot" is colliding
            {
                Available = false;
                //Debug.Log("Bottom Not Available");
                pivotContactCount = 0; // Ensure counter doesn't go negative
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerContactCount--;
            if (playerContactCount <= 0) // Only set PlayerColliding to false if no "Player" is colliding
            {
                PlayerColliding = false;
                //Debug.Log("Player Not Colliding");
                playerContactCount = 0; // Ensure counter doesn't go negative
            }
        }
    }
    public void SpawnToken()
    {
        if (tokenPrefab != null)
        {
            // Get the position of the TopCollider object
            Vector3 spawnPosition = transform.position;       

            // Determine the spawn positions based on fixed offsets
            // Above position (same X, higher Y)
            Vector3 downPosition = spawnPosition + Vector3.down * 1.5f;

            // Left position (left of the object, lower Y)
            Vector3 leftPosition = spawnPosition + Vector3.left + Vector3.down * 0.5f;

            // Right position (right of the object, lower Y)
            Vector3 rightPosition = spawnPosition + Vector3.right + Vector3.down * 0.5f;

            // Randomly choose one of the positions
            int randomIndex = Random.Range(0, 3); // 0: top, 1: left, 2: right

            switch (randomIndex)
            {
                case 0:
                    Instantiate(tokenPrefab, downPosition, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(tokenPrefab, rightPosition, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(tokenPrefab, leftPosition, Quaternion.identity);
                    break;
                default:
                    Debug.LogError("Unexpected random index: " + randomIndex);
                    break;
            }
        }
        else
        {
            Debug.LogError("Token prefab not assigned in TopCollider script.");
        }
    }
}
