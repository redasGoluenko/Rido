using UnityEngine;

public class FixedTimeTrail : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    private float decreaseAmount = 0.015f; // Amount to decrease time per press

    void Start()
    {
        // Ensure trailRenderer is assigned
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Decrease the time value of the trailRenderer
            trailRenderer.time -= decreaseAmount;

            // Clamp the time value to ensure it doesn't go below zero
            if (trailRenderer.time < 0)
            {
                trailRenderer.time = 0;
            }
        }
    }
}
