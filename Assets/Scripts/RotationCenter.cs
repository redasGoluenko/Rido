using System.Collections;
using UnityEngine;

public class RotationCenter : MonoBehaviour
{
    public TopCollider topCollider;
    public BottomCollider bottomCollider;
    public LeftCollider leftCollider;
    public RightCollider rightCollider;
    public Rotate rotate;

    // Define the size of a smaller grid block in Unity units  

    // Flags to prevent multiple movements within the cooldown period
    private bool isCooldown = false;

    // Start is called before the first frame update
    void Start()
    {

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
                MoveInstantly(Vector3.up, 2);
                StartCoroutine(StartCooldown());
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                MoveInstantly(Vector3.down, 2);
                StartCoroutine(StartCooldown());
            }
            else if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                MoveInstantly(Vector3.left, 2);
                StartCoroutine(StartCooldown());
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {
                MoveInstantly(Vector3.right, 2);
                StartCoroutine(StartCooldown());
            }
            else
            {
                //destroy the player
                Destroy(rotate.gameObject);
            }
        }
    }

    // Helper method to move the object instantly in a specified direction by a specified number of smaller grid blocks
    void MoveInstantly(Vector3 direction, int numberOfBlocks)
    {
        // Calculate the distance to move based on the smaller grid size
        float distance = numberOfBlocks;

        // Calculate the target position based on the specified direction and distance
        Vector3 targetPosition = transform.position + direction * distance;

        // Move the object instantly to the target position
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

        // Wait for 5 seconds before allowing another movement
        yield return new WaitForSeconds(0.25f);

        // Reset cooldown flag to false
        isCooldown = false;
    }
}
