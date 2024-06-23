using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotationCenter; // Reference to the object we want to rotate around
    private float rotateSpeed = 150f; // Speed of rotation in degrees per second
    private float initialRotateSpeed = 150f;
    public bool clockwise = true; // Direction of rotation
    public float desiredDistance = 5f; // The desired distance from the rotation center
    public float correctionSpeed = 2f; // Speed at which the distance correction happens
    public bool isCollidingWithToken = false; // Flag to track collision with objects tagged as "Token"
    private GameObject currentToken; // Reference to the currently collided token
    private float lastTokenDestructionTime = -1f; // Time when the last token was destroyed
    public float gracePeriod = 0.5f; // Grace period in seconds to ignore brief multiple token situations
    public int tokenCount = 0; // Number of tokens picked up by the player
    private int previousTokenCount = 0;
    public Ease ease; // Reference to the Ease script
    public Camera cam;
    public TokenCounter tokenCounter;
    public bool isCollidingWithRedirectToken = false; // Flag to track collision with objects tagged as "RedirectToken"
    public bool isCollidingWithHoldToken = false; // Flag to track collision with objects tagged as "HoldToken"
    public bool leftHoldToken = false; // Flag to track if the player is holding a token
    public bool pastThirty = false; // Flag to track if the player has picked up more than 50 tokens
    public bool pastSixty = false; // Flag to track if the player has picked up more than 60 tokens                                 // 

    private void Start()
    {
        pastThirty = false;  
        pastSixty = false;

        previousTokenCount = tokenCount;
        if (cam != null)
        {
            cam.backgroundColor = Color.grey;
        }

        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        pastThirty = tokenCount > 30 ? true : false; // Check if the player has picked up more than 50 tokens
        pastSixty = tokenCount > 60 ? true : false; // Check if the player has picked up more than 60 tokens

        if (tokenCount != previousTokenCount)
        {
            Debug.Log(rotateSpeed);
            // Update rotateSpeed based on tokenCount
            rotateSpeed = initialRotateSpeed + (tokenCount / 2);

            // Update previousTokenCount to current tokenCount
            previousTokenCount = tokenCount;
        }

        // Ensure rotationCenter is assigned
        if (rotationCenter == null)
        {
            return;
        }
        // Orbit around the rotationCenter
        OrbitAround();

        // Adjust the distance to the desired distance
        CorrectDistance();

        // Check for touch input and destroy token if colliding with it
        if ((isCollidingWithToken || isCollidingWithRedirectToken) && Input.touchCount > 0)
        {
            // Destroy the token
            Destroy(currentToken);
            StartCoroutine(FlashBackground());
            tokenCount++;
            // Record the time of token destruction
            lastTokenDestructionTime = Time.time;
            // Reset the flag and reference after destroying the token
            isCollidingWithToken = false;
            isCollidingWithRedirectToken = false;       
            currentToken = null;
        }

        // Check if there are multiple tokens in the scene after the grace period
        if (Time.time - lastTokenDestructionTime > gracePeriod && CountTokens() > 1)
        {
            // Handle player death (e.g., deactivate player or trigger a game-over event)
            Debug.Log("Player dies due to multiple tokens in the scene.");
            Die();
        }
    }

    void OrbitAround()
    {
        // Calculate the orbit movement
        Vector3 relativePos = transform.position - rotationCenter.position;

        // Determine rotation direction based on clockwise boolean
        float direction = clockwise ? 1f : -1f;

        Quaternion rotation = Quaternion.Euler(0, 0, direction * rotateSpeed * Time.deltaTime);
        relativePos = rotation * relativePos;
        transform.position = rotationCenter.position + relativePos;
    }

    void CorrectDistance()
    {
        // Calculate the current distance from the rotation center
        Vector3 toCenter = transform.position - rotationCenter.position;
        float currentDistance = toCenter.magnitude;

        // Check if the distance is different from the desired distance
        if (Mathf.Abs(currentDistance - desiredDistance) > 0.01f) // Adding a small tolerance
        {
            // Calculate the direction to adjust the position
            Vector3 direction = toCenter.normalized;

            // Interpolate towards the correct distance
            Vector3 targetPosition = rotationCenter.position + direction * desiredDistance;

            // Move the object slightly towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, correctionSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            tokenCounter.ColorYellow();
            isCollidingWithToken = true; // Flag to track collision state
            currentToken = collision.gameObject; // Store the reference to the collided token
        }
        if(collision.gameObject.CompareTag("RedirectToken"))
        {
            tokenCounter.ColorBlue();
            isCollidingWithRedirectToken = true; // Flag to track collision state
            currentToken = collision.gameObject; // Store the reference to the collided token
        }    
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            tokenCounter.ColorPurple();        
            isCollidingWithHoldToken = true; // Flag to track collision state
            tokenCount++; // Increment the token count
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            tokenCounter.ColorWhite();
            // Check if there was no touch input when the collision with the token ended
            if (Input.touchCount == 0)
            {             
                Die();
            }
            // Reset the collision flag and reference
            isCollidingWithToken = false;
            currentToken = null;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            tokenCounter.ColorWhite();
            if (Input.touchCount == 0)
            {
                Die();
            }

            isCollidingWithRedirectToken = false; // Reset the collision flag
            currentToken = null; // Reset the reference to the collided token
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            tokenCounter.ColorWhite();
            isCollidingWithHoldToken = false; // Reset the collision flag
            leftHoldToken = true; // Set the leftHoldToken flag to true         
        }
    }
    IEnumerator FlashBackground()
    {
        if (cam == null) yield break;

        Color flashColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        float flashDuration = 0.2f; // The duration of the flash effect
        float zoomDuration = 0.07f; // The duration of the zoom effect
        float zoomFactor = 0.95f; // Amount by which to zoom in, e.g., half the current size
        float elapsedTime = 0f;

        // Store the original background color and camera size
        Color originalColor = cam.backgroundColor;
        float originalSize = cam.orthographicSize;

        // Calculate the target camera size for zooming in
        float targetSize = originalSize * zoomFactor;

        // Change background to flash color
        cam.backgroundColor = flashColor;

        // Interpolate to the target zoom size and back to the original color
        while (elapsedTime < flashDuration)
        {
            cam.backgroundColor = Color.Lerp(flashColor, originalColor, elapsedTime / flashDuration);
            cam.orthographicSize = Mathf.Lerp(originalSize, targetSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset elapsed time for zoom out effect
        elapsedTime = 0f;

        // Ensure the background color is exactly the original and zoom out the camera smoothly
        cam.backgroundColor = originalColor;

        while (elapsedTime < zoomDuration)
        {
            cam.orthographicSize = Mathf.Lerp(targetSize, originalSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera size is exactly the original at the end
        cam.orthographicSize = originalSize;
    }

    // Method to count the number of active tokens in the scene
    int CountTokens()
    {
        // Find all GameObjects tagged as "Token"
        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        GameObject[] redirectTokens = GameObject.FindGameObjectsWithTag("RedirectToken");
        // Return the count of these objects
        return tokens.Length + redirectTokens.Length;
    }
    public void Die()
    {       
            if (cam != null)
            {
                // Detach the camera from the player
                cam.transform.SetParent(null);
            }

            // Deactivate the player and its children
            gameObject.SetActive(false);

            // Optionally, you could trigger any other death-related logic here, like fading out
            ease.FadeIn();         
    }  
}
