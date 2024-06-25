using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Coroutine zoomCoroutine; // Reference to the zooming coroutine
    private GameObject currentToken; // Reference to the currently collided token
    
    public Ease ease; // Reference to the Ease script
    public Camera cam; // Reference to the Camera component
    public Transform rotationCenter; // Reference to the object we want to rotate around
    public TokenCounter tokenCounter; // Reference to the TokenCounter script
    public TrailRenderer trailRenderer; // Reference to the TrailRenderer component
    public SpinObject2D triangleOne; // Reference to the SpinObject2D script attached to the first star
    public SpinObject2D triangleTwo; // Reference to the SpinObject2D script attached to the second star
    public SpinObject2D triangleThree; // Reference to the SpinObject2D script attached to the third star
    public SpinObject2D triangleFour; // Reference to the SpinObject2D script attached to the fourth star
    public GameObject slopes; // Reference to the Slopes GameObject      

    private Color flashColor; // Color for the flash effect
    private float rotateSpeed = 150f; // Speed of rotation in degrees per second
    private float initialRotateSpeed = 150f; // Initial speed of rotation in degrees per second
    private float lastTokenDestructionTime = -1f; // Time when the last token was destroyed
    private int previousTokenCount = 0; // Number of tokens picked up by the player in the previous frame

    public bool clockwise = true; // Direction of rotation
    public bool isCollidingWithToken = false; // Flag to track collision with objects tagged as "Token"
    public bool isCollidingWithRedirectToken = false; // Flag to track collision with objects tagged as "RedirectToken"
    public bool isCollidingWithHoldToken = false; // Flag to track collision with objects tagged as "HoldToken"
    public bool isCollidingWithRedToken = false;
    public bool leftHoldToken = false; // Flag to track if the player is holding a token
    public bool pastThirty; // Flag to track if the player has picked up more than 30 tokens
    public bool pastSixty; // Flag to track if the player has picked up more than 60 tokens
    public bool pastNinety; // Flag to track if the player has picked up more than 90 tokens  
    public bool dead = false; // Flag to track if the player is dead  

    public float desiredDistance = 5f; // The desired distance from the rotation center
    public float correctionSpeed = 2f; // Speed at which the distance correction happens   
    public float gracePeriod = 0.5f; // Grace period in seconds to ignore brief multiple token situations
    public int tokenCount = 0; // Number of tokens picked up by the player   

    private void Start()
    {
        pastThirty = false;
        pastSixty = false;
        pastNinety = false;

        previousTokenCount = tokenCount;
        
        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateBackgroundColor(); // Update the background color based on the token count
        ManageZoomCoroutine(); // Manage the zooming coroutine
        UpdateTokenCount(); // Update the token count and adjust the rotate speed
        UpdateTokenCounterColor(); // Update the token counter color
        OrbitAround(); // Orbit around the rotation center
        CorrectDistance(); // Adjust the distance to the desired distance
        HandleTokenCollision(); // Handle token collision
        CheckGameOver(); // Check if the player has multiple tokens in the scene

        if (isCollidingWithHoldToken)
        {
            tokenCounter.textMeshPro.fontSize = 100;
            tokenCounter.textMeshPro.color = new Color(0.5f, 0, 0.5f);
            StopCoroutine(tokenCounter.flashingCoroutine);
        }       
        else
        {
            tokenCounter.textMeshPro.fontSize = 200;
            tokenCounter.textMeshPro.color = Color.white;
        }

        // Ensure rotationCenter is assigned
        if (rotationCenter == null)
        {
            return;
        }  
        
        triangleOne.clockwise = triangleTwo.clockwise = triangleThree.clockwise = triangleFour.clockwise = !clockwise;
    }

    void UpdateTokenCounterColor()
    {
        bool isScreenTouched = Input.touchCount > 0;

        if (isCollidingWithHoldToken)
        {
            //Debug.Log("Colliding with hold token");
            tokenCounter.ChangeColor(new Color(0.5f, 0, 0.5f));
        }
        else if (isCollidingWithRedirectToken && isScreenTouched)
        {
            //Debug.Log("Colliding with redirect token");
            tokenCounter.ChangeColor(Color.blue);
        }
        else if (isCollidingWithRedToken && isScreenTouched)
        {
            //Debug.Log("Colliding with red token");
            tokenCounter.ChangeColor(Color.red);
        }
        else if (isCollidingWithToken && isScreenTouched)
        {
            //Debug.Log("Colliding with token");
            tokenCounter.ChangeColor(new Color(1.0f, 0.92f, 0.3f));
        }
    }


    // Method to update the background color based on the token count
    void UpdateBackgroundColor()
    {
        pastThirty = tokenCount > 30 ? true : false;
        pastSixty = tokenCount > 60 ? true : false;
        pastNinety = tokenCount > 90 ? true : false;

        Color lightBlue = new Color(0.7f, 0.85f, 1f);
        Color lightPurple = new Color(0.85f, 0.7f, 1f);

        float colorTransitionSpeed = 2f;

        if (pastThirty && !pastSixty && !pastNinety)
        {
            if (cam != null)
            {
                flashColor = new Color(0.5f, 0.7f, 1f, 1f);
                // Smoothly transition to lightBlue
                cam.backgroundColor = Color.Lerp(cam.backgroundColor, lightBlue, Time.deltaTime * colorTransitionSpeed);
            }
        }
        else if (pastSixty && !pastNinety)
        {
            if (cam != null)
            {
                flashColor = new Color(0.8f, 0.7f, 0.9f, 1f);
                // Smoothly transition to lightPurple
                cam.backgroundColor = Color.Lerp(cam.backgroundColor, lightPurple, Time.deltaTime * colorTransitionSpeed);
            }
        }
        else if(pastNinety)
        {
            if(cam != null)
            {           
                flashColor = new Color(1f, 0.75f, 0.75f, 1f);
                // Smoothly transition to lightPurple
                cam.backgroundColor = Color.Lerp(cam.backgroundColor, new Color(1f, 0.6f, 0.6f, 1f), Time.deltaTime * colorTransitionSpeed);
            }
        }
        else
        {
            if (cam != null)
            {
                flashColor = new Color(1f, 1f, 0.8f, 1f);
                // Smoothly transition back to yellow
                cam.backgroundColor = Color.Lerp(cam.backgroundColor, new Color(1f, 0.96f, 0.7f), Time.deltaTime * colorTransitionSpeed);
            }
        }
    }

    // Method to manage the zooming coroutine
    void ManageZoomCoroutine()
    {
        if (isCollidingWithHoldToken && zoomCoroutine == null && Input.touchCount > 0)
        {
            // Start the zooming coroutine if it's not already running
            zoomCoroutine = StartCoroutine(ContinuousZoomInAndBack());
        }
        else if (!isCollidingWithHoldToken && zoomCoroutine != null)
        {
            // Don't stop the coroutine immediately; it will handle zooming out by itself
            zoomCoroutine = null;
        }
    }

    // Method to update the token count and adjust the rotate speed
    void UpdateTokenCount()
    {
        if (tokenCount != previousTokenCount)
        {
            //Debug.Log(rotateSpeed);
            // Update rotateSpeed based on tokenCount
            rotateSpeed = initialRotateSpeed + tokenCount;

            // Update previousTokenCount to current tokenCount
            previousTokenCount = tokenCount;
        }
    }

    // Method to handle token collision
    void HandleTokenCollision()
    {
        if ((isCollidingWithToken || isCollidingWithRedirectToken || isCollidingWithRedToken) && Input.touchCount > 0)
        {
            // Destroy the token
            Destroy(currentToken);
            StartCoroutine(FlashBackground(flashColor));
            tokenCount++;
            // Record the time of token destruction
            lastTokenDestructionTime = Time.time;
            // Reset the flag and reference after destroying the token
            isCollidingWithToken = false;
            isCollidingWithRedirectToken = false;
            currentToken = null;
        }
    }

    // Method to check if the player has multiple tokens in the scene
    void CheckGameOver()
    {
        if (Time.time - lastTokenDestructionTime > gracePeriod && CountTokens() > 1)
        {
            // Handle player death (e.g., deactivate player or trigger a game-over event)
            Debug.Log("Player dies due to multiple tokens in the scene.");
            Die();
        }
    }

    // Method to orbit around the rotation center
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

    // Method to adjust the distance to the desired distance
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

    // Method to handle collision with tokens
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {                    
            isCollidingWithToken = true; // Flag to track collision state
            currentToken = collision.gameObject; // Store the reference to the collided token
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {           
            isCollidingWithRedirectToken = true; // Flag to track collision state
            currentToken = collision.gameObject; // Store the reference to the collided token
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {           
            isCollidingWithHoldToken = true; // Flag to track collision state
            tokenCount++; // Increment the token count                             
        }
        if(collision.gameObject.CompareTag("RedToken"))
        {          
            isCollidingWithRedToken = true;
            currentToken = collision.gameObject;
        }
    }

    // Method to handle collision exit with tokens
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {         
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
            if (Input.touchCount == 0)
            {
                Die();
            }

            isCollidingWithRedirectToken = false; // Reset the collision flag
            currentToken = null; // Reset the reference to the collided token                   
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {            
            isCollidingWithHoldToken = false; // Reset the collision flag
            leftHoldToken = true; // Set the leftHoldToken flag to true
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {          
            if (Input.touchCount == 0)
            {
                Die();
            }

            isCollidingWithRedToken = false;
            currentToken = null;
            RotateCamera();
        }
    }

    // Flash the background color and zoom in and out when a token is destroyed
    IEnumerator FlashBackground(Color flashColor)
    {
        if (cam == null) yield break;
      
        float flashDuration = 0.15f; // The duration of the flash effect
        float zoomDuration = 0.1f; // The duration of the zoom effect
        float zoomFactor = 0.98f; // Amount by which to zoom in, e.g., half the current size
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

    // Coroutine to continuously zoom in and back when colliding with a hold token
    IEnumerator ContinuousZoomInAndBack()
    {
        if (cam == null) yield break;

        float zoomFactor = 0.95f; // Zoom in factor per frame
        float maxZoomFactor = 0.8f; // Maximum zoom limit (50% of original size)
        float originalSize = cam.orthographicSize; // Store the original camera size
        float zoomOutDuration = 0.2f; // Duration for zooming back to original size
        float newSize;

        // Zoom in while isCollidingHoldToken is true
        while (isCollidingWithHoldToken)
        {
            // Calculate the new size by applying the zoom factor
            newSize = cam.orthographicSize * zoomFactor;

            // Clamp the size to not go beyond the maximum zoom factor
            cam.orthographicSize = Mathf.Max(newSize, originalSize * maxZoomFactor);

            // Wait for the next frame
            yield return new WaitForSeconds(0.025f);
        }

        // Once isCollidingHoldToken becomes false, smoothly zoom back to the original size
        float elapsedTime = 0f;
        float currentSize = cam.orthographicSize;

        while (elapsedTime < zoomOutDuration)
        {
            cam.orthographicSize = Mathf.Lerp(currentSize, originalSize, elapsedTime / zoomOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera size is set to the exact original size
        cam.orthographicSize = originalSize;
    }




    // Method to count the number of active tokens in the scene
    int CountTokens()
    {
        // Find all GameObjects tagged as "Token"
        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        GameObject[] redirectTokens = GameObject.FindGameObjectsWithTag("RedirectToken");
        GameObject[] holdTokens = GameObject.FindGameObjectsWithTag("HoldToken");
        GameObject[] redTokens = GameObject.FindGameObjectsWithTag("RedToken");
        // Return the count of these objects
        return tokens.Length + redirectTokens.Length + holdTokens.Length + redTokens.Length;
    }
    
    // Method to rotate the camera 45 degrees slowly
    void RotateCamera()
    {
        if (cam == null) return;

        float rotationSpeed = 100f; // Speed of rotation in degrees per second
        float targetAngle = cam.transform.eulerAngles.z + 45f;
        // Rotate the camera towards the target angle
        StartCoroutine(RotateCameraCoroutine(targetAngle, rotationSpeed));
    }

    // Coroutine to rotate the camera towards the target angle
    IEnumerator RotateCameraCoroutine(float targetAngle, float rotationSpeed)
    {
        if (cam == null) yield break;

        float currentAngle = cam.transform.eulerAngles.z; // Current angle of the camera
        float elapsedTime = 0f; // Elapsed time since the start of the coroutine

        // Rotate the camera towards the target angle
        while (currentAngle < targetAngle)
        {
            // Calculate the new angle based on the rotation speed
            currentAngle += rotationSpeed * Time.deltaTime;

            // Apply the new angle to the camera rotation
            cam.transform.eulerAngles = new Vector3(0, 0, currentAngle);
            slopes.transform.eulerAngles = new Vector3(0, 0, currentAngle);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the camera rotation is set to the exact target angle
        cam.transform.eulerAngles = new Vector3(0, 0, targetAngle);
        slopes.transform.eulerAngles = new Vector3(0, 0, targetAngle);
    }
    // Method to handle player death
    public void Die()
    {              
        // Deactivate the player and its children
        gameObject.SetActive(false);   
        dead = true; // Set the dead flag to true

        // Optionally, you could trigger any other death-related logic here, like fading out
        ease.FadeIn();     
        tokenCounter.textMeshPro.color = Color.white;       
    }
}
