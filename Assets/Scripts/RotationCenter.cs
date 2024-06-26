using System.Collections;
using UnityEngine;

public class RotationCenter : MonoBehaviour
{
    public TopCollider topCollider; // Reference to the TopCollider script
    public BottomCollider bottomCollider; // Reference to the BottomCollider script
    public LeftCollider leftCollider; // Reference to the LeftCollider script
    public RightCollider rightCollider; // Reference to the RightCollider script
    public Rotate rotate; // Reference to the Rotate script

    private bool flag = true; // Flag to prevent multiple rotations in the same frame
    private bool isCooldown = false; // Flag to prevent multiple movements in quick succession
    private float moveDistance = 2.0f; // Distance to move the object

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
        if (!rotate.isMenu)
        {
            HandleHoldToken(); // Handle the hold token movement
            HandleTokens(); // Handle the standard, redirect and red token movement
        }
        
    }
    public void SimulatePressWithDelay()
    {
        StartCoroutine(DelayedSimulatePress(0.125f)); // 1f is the delay in seconds
    }

    private IEnumerator DelayedSimulatePress(float delay)
    {
        yield return new WaitForSeconds(delay);
        SimulatePress();
        rotate.DestroyCurrentToken();
        rotate.tokenCount++;
    }

    public void SimulatePress()
    {
        rotate.clockwise = !rotate.clockwise;
        if (topCollider.Available && topCollider.PlayerColliding)
        {
            topCollider.SpawnToken();
            Teleport(Vector3.up, moveDistance);
            StartCoroutine(StartCooldown());
        }
        else if (bottomCollider.Available && bottomCollider.PlayerColliding)
        {
            bottomCollider.SpawnToken();
            Teleport(Vector3.down, moveDistance);
            StartCoroutine(StartCooldown());
        }
        else if (leftCollider.Available && leftCollider.PlayerColliding)
        {
            leftCollider.SpawnToken();
            Teleport(Vector3.left, moveDistance);
            StartCoroutine(StartCooldown());
        }
        else if (rightCollider.Available && rightCollider.PlayerColliding)
        {
            rightCollider.SpawnToken();
            Teleport(Vector3.right, moveDistance);
            StartCoroutine(StartCooldown());
        }       
    }
    // Method to handle the standard,redirect and red token movement
    void HandleTokens()
    {
        // Check for movement in each direction based on collider availability and player collision
        if (!isCooldown && IsScreenTouched() && !rotate.isCollidingWithHoldToken)
        {
            rotate.clockwise = !rotate.clockwise;

            if (topCollider.Available && topCollider.PlayerColliding)
            {
                if (rotate.isCollidingWithRedirectToken)
                {
                    topCollider.SpawnTokenRedirect();  
                }
                else if (rotate.isCollidingWithRedToken)
                {
                    if (Random.Range(1, 3) == 1)
                    {                      
                        topCollider.SpawnToken();
                        Teleport(Vector3.up, moveDistance);
                    }
                    else
                    {                       
                        topCollider.SpawnTokenRedirect();
                    }
                }
                else
                {
                    topCollider.SpawnToken();
                    Teleport(Vector3.up, moveDistance);
                }
                StartCoroutine(StartCooldown());
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                if (rotate.isCollidingWithRedirectToken)
                {
                    bottomCollider.SpawnTokenRedirect(); 
                }
                else if (rotate.isCollidingWithRedToken)
                {
                    if (Random.Range(1, 3) == 1)
                    {                        
                        bottomCollider.SpawnToken();
                        Teleport(Vector3.down, moveDistance);
                    }
                    else
                    {
                        bottomCollider.SpawnTokenRedirect();
                    }
                }
                else
                {
                    bottomCollider.SpawnToken();
                    Teleport(Vector3.down, moveDistance);
                }
                StartCoroutine(StartCooldown());
            }
            else if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                if (rotate.isCollidingWithRedirectToken)
                {
                    leftCollider.SpawnTokenRedirect();     
                }
                else if (rotate.isCollidingWithRedToken)
                {                   
                    if(Random.Range(1, 3) == 1)
                    {                       
                        leftCollider.SpawnToken();
                        Teleport(Vector3.left, moveDistance);
                    } 
                    else
                    {                    
                        leftCollider.SpawnTokenRedirect();
                    }                  
                }
                else
                {
                    leftCollider.SpawnToken();
                    Teleport(Vector3.left, moveDistance);
                }
                StartCoroutine(StartCooldown());
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {
                if (rotate.isCollidingWithRedirectToken)
                {
                    rightCollider.SpawnTokenRedirect();
                }
                else if (rotate.isCollidingWithRedToken)
                {
                    if (Random.Range(1, 3) == 1)
                    {                        
                        rightCollider.SpawnToken();
                        Teleport(Vector3.right, moveDistance);
                    }
                    else
                    {                      
                        rightCollider.SpawnTokenRedirect();
                    }
                }
                else
                {
                    rightCollider.SpawnToken();
                    Teleport(Vector3.right, moveDistance);
                }
                StartCoroutine(StartCooldown());
            }
            else
            {
                rotate.Die();
            }
        }
    }

    // Method to handle the hold token movement
    void HandleHoldToken()
    {
        if (rotate.isCollidingWithHoldToken && Input.touchCount > 0 && flag)
        {
            flag = false;

            if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;               
                Teleport(Vector3.left, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;               
                Teleport(Vector3.right, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (topCollider.Available && topCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;              
                Teleport(Vector3.up, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;          
                Teleport(Vector3.down, moveDistance);
                StartCoroutine(StartCooldown());
            }
        }

        if (rotate.leftHoldToken)
        {
            flag = true;
            rotate.leftHoldToken = false;

            if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                leftCollider.SpawnToken();
                Teleport(Vector3.left, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                rightCollider.SpawnToken();
                Teleport(Vector3.right, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (topCollider.Available && topCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                topCollider.SpawnToken();
                Teleport(Vector3.up, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                bottomCollider.SpawnToken();
                Teleport(Vector3.down, moveDistance);
                StartCoroutine(StartCooldown());
            }
        }
    }

    // Method to instantly teleport the object to a new position based on direction and distance
    void Teleport(Vector3 direction, float distance)
    {
        // Calculate the target position based on the specified direction and distance
        Vector3 targetPosition = transform.position + direction * distance;

        // Set the position instantly to the target position
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
