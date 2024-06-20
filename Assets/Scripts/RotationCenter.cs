using System.Collections;
using UnityEngine;

public class RotationCenter : MonoBehaviour
{
    public TopCollider topCollider;
    public BottomCollider bottomCollider;
    public LeftCollider leftCollider;
    public RightCollider rightCollider;
    public Rotate rotate;

    // Flags to prevent multiple movements within the cooldown period
    private bool isCooldown = false;

    // Speed of movement (distance per second)
    private float moveSpeed = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {      
        topCollider.GetComponent<SpriteRenderer>().enabled = false;   
        bottomCollider.GetComponent<SpriteRenderer>().enabled = false;    
        leftCollider.GetComponent<SpriteRenderer>().enabled = false;       
        rightCollider.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for movement in each direction based on collider availability and player collision
        if (!isCooldown && IsScreenTouched())
        {        
            rotate.clockwise = !rotate.clockwise;
            if (topCollider.Available && topCollider.PlayerColliding)
            {
                StartCoroutine(MoveSmoothly(Vector3.up, 2));
                StartCoroutine(StartCooldown());
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                StartCoroutine(MoveSmoothly(Vector3.down, 2));
                StartCoroutine(StartCooldown());
            }
            else if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                StartCoroutine(MoveSmoothly(Vector3.left, 2));
                StartCoroutine(StartCooldown());
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {
                StartCoroutine(MoveSmoothly(Vector3.right, 2));
                StartCoroutine(StartCooldown());
            }
            else
            {             
                Destroy(rotate.gameObject);
            }
        }
    }

    // Coroutine to move the object smoothly in a specified direction over time
    IEnumerator MoveSmoothly(Vector3 direction, int numberOfBlocks)
    {
        // Calculate the distance to move based on the smaller grid size
        float distance = numberOfBlocks;

        // Calculate the target position based on the specified direction and distance
        Vector3 targetPosition = transform.position + direction * distance;

        // Calculate the duration based on the move speed
        float duration = distance / moveSpeed;

        // Store the starting position for interpolation
        Vector3 startPosition = transform.position;

        // Time elapsed while moving
        float elapsed = 0;

        // Interpolate position over time
        while (elapsed < duration)
        {
            // Calculate interpolation ratio
            float t = elapsed / duration;

            // Move towards the target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Update elapsed time
            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure final position is exactly at the target
        transform.position = targetPosition;
    }

    // Helper method to check if the screen is being touched
    bool IsScreenTouched()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            // Get the first touch (could also iterate through multiple touches if needed)
            Touch touch = Input.GetTouch(0);

            // Check if the touch just began, moved, or is stationary
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                return true;
            }
        }

        // Return false if no touches are detected or no relevant touch phases are detected
        return false;
    }

    // Coroutine to start the cooldown period after a movement
    IEnumerator StartCooldown()
    {
        // Set cooldown flag to true
        isCooldown = true;

        // Wait for 0.25 seconds before allowing another movement
        yield return new WaitForSeconds(0.25f);

        // Reset cooldown flag to false
        isCooldown = false;
    }
}
