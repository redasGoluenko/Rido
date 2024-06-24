using UnityEngine;

public class FollowOnTouch : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player's transform

    private Vector3 initialPosition; // To store the initial position of the object
    private Vector3 previousPosition; // Previous frame's position of the object

    private bool isFollowing = false; // State to track if the object should follow the player
    private bool hasSkipped = false; // State to track if the object has skipped the player

    private float returnThreshold = 0.5f; // Distance threshold to consider the object "returned" to its initial position
    private float totalDistanceMoved = 0f; // Total distance moved by the object
    
    private void Start()
    {
        // Store the initial position when the script starts
        initialPosition = transform.position;
        previousPosition = initialPosition; // Initialize previous position
    }

    private void Update()
    {
       HandleTouchInputAndInteraction(); // Handle touch input and interaction logic
    }

    // Handle touch input and interaction logic
    void HandleTouchInputAndInteraction()
    {
        // Check if there is at least one touch on the screen
        if (Input.touchCount > 0)
        {
            // Get the first touch (usually enough for simple checks)
            Touch touch = Input.GetTouch(0);

            // Check if the touch is in progress (finger is on the screen)
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // Calculate movement distance from previous position
                float distance = Vector3.Distance(transform.position, previousPosition);
                totalDistanceMoved += distance; // Accumulate total distance moved

                // Update previous position to current position
                previousPosition = transform.position;

                // If the object should follow the player, update its position
                if (isFollowing && playerTransform != null)
                {
                    // Make the object follow the player's position
                    transform.position = playerTransform.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // If the touch ended or was canceled, handle the follow logic
                if (isFollowing)
                {
                    isFollowing = false;

                    // Check if the object is close to its initial position
                    if (Vector3.Distance(transform.position, initialPosition) > returnThreshold)
                    {
                        // Try to call the Die method on the player if the object is not close to its initial position
                        if (playerTransform != null)
                        {
                            // Replace "Rotate" with the actual class name containing Die()
                            var playerScript = playerTransform.GetComponent<Rotate>();
                            if (playerScript != null)
                            {
                                playerScript.Die(); // Call the Die() method                              
                            }
                        }
                    }
                    else
                    {
                        // Destroy the object if it's close to its initial position
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    // Detect collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the player's transform
            playerTransform = collision.transform;

            // Set the state to start following the player
            isFollowing = true;           
            if(Input.touchCount == 0)
            {
                hasSkipped = true;
            }
        }
    }

    // Detect when the object stops colliding with the player
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasSkipped)
            {
                var playerScript = playerTransform.GetComponent<Rotate>();
                playerScript.Die();                
            }
        }
        hasSkipped = false;
    }

    // Handle cleanup when the object is destroyed
    private void OnDestroy()
    {
        //Debug.Log($"Distance moved {totalDistanceMoved}");
        var playerScript = playerTransform.GetComponent<Rotate>();
        if (totalDistanceMoved < 0.7f || totalDistanceMoved > 7.5f)
        {
            playerScript.Die();       
        }
    }
}
