using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailabilityIndicator : MonoBehaviour
{
    private bool isCollidingWithCollider = false; // Flag to track collision with objects tagged as "Collider"
    private bool isCollidingWithPivot = false;   // Flag to track collision with objects tagged as "Pivot"
    private GameObject currentCollidingPivot;     // Reference to the current colliding pivot object

    private Color originalColor = Color.black; // Original color of the pivot object
    private Color targetColor = Color.black;   // The target color to transition to
    private float colorChangeSpeed = 3f;      // Speed of color change

    void Start()
    {
        //make invisible
        GetComponent<SpriteRenderer>().enabled = false;
        // Store the original color of the pivot object
        SpriteRenderer pivotRenderer = GetComponent<SpriteRenderer>();
        if (pivotRenderer != null)
        {
            originalColor = pivotRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");
        }
    }

    void Update()
    {
        // Smoothly transition the color of the current colliding pivot object
        if (currentCollidingPivot != null)
        {
            SpriteRenderer pivotRenderer = currentCollidingPivot.GetComponent<SpriteRenderer>();
            if (pivotRenderer != null)
            {
                pivotRenderer.color = Color.Lerp(pivotRenderer.color, targetColor, colorChangeSpeed * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collider"))
        {
            isCollidingWithCollider = true;
        }
        if (collision.gameObject.CompareTag("Pivot"))
        {
            isCollidingWithPivot = true;
            currentCollidingPivot = collision.gameObject;
        }

        // Check if both conditions are true
        if (isCollidingWithCollider && isCollidingWithPivot)
        {
            // Change the target color of the current colliding pivot object to green
            targetColor = Color.green;
        }
        else
        {
            // Change the target color to black if not both conditions are met
            targetColor = Color.black;
        }

        // Update the color immediately
        UpdatePivotColor();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collider"))
        {
            isCollidingWithCollider = false;
        }
        if (collision.gameObject.CompareTag("Pivot") && collision.gameObject == currentCollidingPivot)
        {
            isCollidingWithPivot = false;
            currentCollidingPivot = null;
        }

        // Check if either condition is false
        if (!isCollidingWithCollider || !isCollidingWithPivot)
        {
            // Change the target color to black
            targetColor = Color.black;
        }

        // Update the color immediately
        UpdatePivotColor();
    }

    void UpdatePivotColor()
    {
        // Reset the color of all pivot objects
        SpriteRenderer[] pivotRenderers = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        foreach (SpriteRenderer renderer in pivotRenderers)
        {
            if (renderer.gameObject.CompareTag("Pivot"))
            {
                renderer.color = originalColor;
            }
        }

        // Transition the color of the current colliding pivot object
        if (currentCollidingPivot != null)
        {
            SpriteRenderer pivotRenderer = currentCollidingPivot.GetComponent<SpriteRenderer>();
            if (pivotRenderer != null)
            {
                pivotRenderer.color = targetColor;
            }
        }
    }
}
