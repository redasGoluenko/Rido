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

    // Distance to move per step (number of blocks)
    private float moveDistance = 2.0f;

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
        if (rotate.leftHoldToken)
        {
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
            else if(topCollider.Available && topCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                topCollider.SpawnToken();
                Teleport(Vector3.up, moveDistance);
                StartCoroutine(StartCooldown());
            }
            else if(bottomCollider.Available && bottomCollider.PlayerColliding)
            {
                rotate.clockwise = !rotate.clockwise;
                bottomCollider.SpawnToken();
                Teleport(Vector3.down, moveDistance);
                StartCoroutine(StartCooldown());
            }                    
        }
        // Check for movement in each direction based on collider availability and player collision
        if (!isCooldown && IsScreenTouched() && !rotate.isCollidingWithHoldToken)
        {
            rotate.clockwise = !rotate.clockwise;
                       
            if (topCollider.Available && topCollider.PlayerColliding)
            {                      
                if(!rotate.isCollidingWithRedirectToken)
                {                              
                   topCollider.SpawnToken();
                   Teleport(Vector3.up, moveDistance);
                }
                else
                {
                    topCollider.SpawnTokenRedirect();
                }
                StartCoroutine(StartCooldown());              
            }
            else if (bottomCollider.Available && bottomCollider.PlayerColliding)
            {               
                if(!rotate.isCollidingWithRedirectToken)
                {                 
                    bottomCollider.SpawnToken();                  
                    Teleport(Vector3.down, moveDistance);
                }
                else
                {
                    bottomCollider.SpawnTokenRedirect();
                }
                StartCoroutine(StartCooldown());            
            }
            else if (leftCollider.Available && leftCollider.PlayerColliding)
            {
                if (!rotate.isCollidingWithRedirectToken)
                {              
                    leftCollider.SpawnToken();                   
                    Teleport(Vector3.left, moveDistance);
                }
                else
                {
                    leftCollider.SpawnTokenRedirect();
                }                                                                        
                StartCoroutine(StartCooldown());            
            }
            else if (rightCollider.Available && rightCollider.PlayerColliding)
            {          
                if(!rotate.isCollidingWithRedirectToken)
                {                
                    rightCollider.SpawnToken();                
                    Teleport(Vector3.right, moveDistance);
                }
                else
                {
                    rightCollider.SpawnTokenRedirect();
                }
                StartCoroutine(StartCooldown());   
            }
            else
            {
                rotate.Die();
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
