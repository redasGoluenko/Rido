using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    public bool Available = false;
    public bool PlayerColliding = false;

    private int pivotContactCount = 0; // Counter for "Pivot" collisions
    private int playerContactCount = 0; // Counter for "Player" collisions

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
}
