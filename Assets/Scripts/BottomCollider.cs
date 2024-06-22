using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    public bool Available = false;
    public bool PlayerColliding = false;

    private int pivotContactCount = 0; // Counter for "Pivot" collisions
    private int playerContactCount = 0; // Counter for "Player" collisions

    public Rotate rotate;
    public GameObject tokenPrefab;
    public GameObject redirectTokenPrefab;

    private GameObject currentToken;
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
        currentToken = RandomToken();
        if (currentToken != null)
        {
            // Get the position of the TopCollider object
            Vector3 spawnPosition = transform.position;

            // Determine the spawn positions based on fixed offsets
            Vector3 downPosition = spawnPosition + Vector3.down * 1.5f;
            Vector3 leftPosition = spawnPosition + Vector3.left + Vector3.down * 0.5f;
            Vector3 rightPosition = spawnPosition + Vector3.right + Vector3.down * 0.5f;

            // List of potential spawn positions
            List<Vector3> potentialPositions = new List<Vector3> { downPosition, leftPosition, rightPosition };

            // Shuffle the potential positions to introduce randomness
            for (int i = 0; i < potentialPositions.Count; i++)
            {
                int randomIndex = Random.Range(i, potentialPositions.Count);
                Vector3 temp = potentialPositions[i];
                potentialPositions[i] = potentialPositions[randomIndex];
                potentialPositions[randomIndex] = temp;
            }

            // Layer mask to check for specific layers or tags (for example, "Obstacle" layer)
            int layerMask = LayerMask.GetMask("Obstacle");

            // Radius or size of the area to check for collisions
            float checkRadius = 0.5f; // Adjust based on your token size

            // Iterate through the positions to find a valid one
            foreach (Vector3 position in potentialPositions)
            {
                // Check if the position is not occupied
                if (!Physics2D.OverlapCircle(position, checkRadius, layerMask))
                {
                    // Spawn the token at the first valid position
                    Instantiate(currentToken, position, Quaternion.identity);
                    return; // Exit after spawning
                }
            }

            // If all positions are occupied, log an error or handle accordingly
            Debug.LogError("All spawn positions are occupied.");
        }
        else
        {
            Debug.LogError("Token prefab not assigned in TopCollider script.");
        }
    }
    public void SpawnTokenRedirect()
    {
        currentToken = RandomToken();
        if (currentToken != null)
        {
            // Get the position of the TopCollider object
            Vector3 spawnPosition = transform.position;

            // Determine the spawn positions based on fixed offsets
            Vector3 topPosition = spawnPosition + Vector3.up * 2.5f;
            Vector3 leftPosition = spawnPosition + Vector3.left + Vector3.up * 1.5f;
            Vector3 rightPosition = spawnPosition + Vector3.right + Vector3.up * 1.5f;

            // List of potential spawn positions
            List<Vector3> potentialPositions = new List<Vector3> { topPosition, leftPosition, rightPosition };

            // Shuffle the potential positions to introduce randomness
            for (int i = 0; i < potentialPositions.Count; i++)
            {
                int randomIndex = Random.Range(i, potentialPositions.Count);
                Vector3 temp = potentialPositions[i];
                potentialPositions[i] = potentialPositions[randomIndex];
                potentialPositions[randomIndex] = temp;
            }

            // Layer mask to check for specific layers or tags (for example, "Obstacle" layer)
            int layerMask = LayerMask.GetMask("Obstacle");

            // Radius or size of the area to check for collisions
            float checkRadius = 0.5f; // Adjust based on your token size

            // Iterate through the positions to find a valid one
            foreach (Vector3 position in potentialPositions)
            {
                // Check if the position is not occupied
                if (!Physics2D.OverlapCircle(position, checkRadius, layerMask))
                {
                    // Spawn the token at the first valid position
                    Instantiate(currentToken, position, Quaternion.identity);
                    return; // Exit after spawning
                }
            }

            // If all positions are occupied, log an error or handle accordingly
            Debug.LogError("All spawn positions are occupied (Bottom Collider).");
        }
        else
        {
            Debug.LogError("Token prefab not assigned in TopCollider script.");
        }
    }
    public GameObject RandomToken()
    {
       GameObject[] tokens = { tokenPrefab, redirectTokenPrefab };
       return rotate.pastThirty ? tokens[Random.Range(0, tokens.Length)] : tokenPrefab;
    }
}
