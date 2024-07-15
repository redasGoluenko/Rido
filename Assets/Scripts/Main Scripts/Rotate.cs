using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rotate : MonoBehaviour
{
    private Coroutine zoomCoroutine; // Reference to the zooming coroutine
    private Coroutine scaleCoroutine; // Reference to the scaling coroutine
    private GameObject currentToken; // Reference to the currently collided token
    private GameObject glowInstance; // Reference to the current token prefab

    public Ease ease; // Reference to the Ease script
    public Camera cam; // Reference to the Camera component
    public Transform rotationCenter; // Reference to the object we want to rotate around
    public RotationCenter rotationCenterScript; // Reference to the RotationCenter script
    public TokenCounter tokenCounter; // Reference to the TokenCounter script
    public TrailRenderer trailRenderer; // Reference to the TrailRenderer component
    public SpinObject2D triangleOne; // Reference to the SpinObject2D script attached to the first star
    public SpinObject2D triangleTwo; // Reference to the SpinObject2D script attached to the second star
    public SpinObject2D triangleThree; // Reference to the SpinObject2D script attached to the third star
    public SpinObject2D triangleFour; // Reference to the SpinObject2D script attached to the fourth star
    public GameObject slopes; // Reference to the Slopes GameObject
    public GoldTokenCurrent goldTokenCurrent; // Reference to the GoldTokenCurrent script
    public BlueTokenCurrent blueTokenCurrent; // Reference to the BlueTokenCurrent script
    public RedTokenCurrent redTokenCurrent; // Reference to the RedTokenCurrent script
    public PurpleTokenCurrent purpleTokenCurrent; // Reference to the PurpleTokenCurrent script
    public XPValue XPValue; // Reference to the XPValue script
    public GameObject SLOT1; // Reference to the Glow GameObject 
    public GameObject SLOT2;
    public GameObject SLOT3;
    public GameObject SLOT4;
    public GameObject SLOT5;

    private Color flashColor; // Color for the flash effect  
    private float initialRotateSpeed = 150f; // Initial speed of rotation in degrees per second
    private float lastTokenDestructionTime = -1f; // Time when the last token was destroyed
    private int previousTokenCount = 0; // Number of tokens picked up by the player in the previous frame
    private int currentGlow; // Current glow number  
    private bool inGlowSelection = false; // Flag to track if the player is in the glow selection menu

    public float rotateSpeed = 150f; // Speed of rotation in degrees per second
    public float UnitXPValue;
    public int currentLevel;
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
    public bool isMenu = false; // Flag to track if the camera is moving
    public int goldTokenCount = 0; // Number of gold tokens picked up by the player
    public int blueTokenCount = 0; // Number of blue tokens picked up by the player
    public int redTokenCount = 0; // Number of red tokens picked up by the player
    public int purpleTokenCount = 0; // Number of purple tokens picked up by the player
    public float desiredDistance = 5f; // The desired distance from the rotation center
    public float correctionSpeed = 2f; // Speed at which the distance correction happens
    public float gracePeriod = 0.5f; // Grace period in seconds to ignore brief multiple token situations
    public int tokenCount = 0; // Number of tokens picked up by the player

    private void Start()
    {
        AudioManager.Instance.SFX.volume = 0.2f;
        pastThirty = false;
        pastSixty = false;
        pastNinety = false;

        if (SceneManager.GetActiveScene().name == "Glows")
        {
            inGlowSelection = true;        
            HandleGlow();
        }          
        currentGlow = PlayerManager.instance.glowNumber;
        if (currentGlow == 4)
        {
            trailRenderer.time = 0.4f;
        }
        else
        {
            trailRenderer.time = 0.3f;
        }
        Destroy(glowInstance);
        ChangeTrailColorUsingGradient(Color.black, 1);  
        HandleGlow();
        currentLevel = PlayerManager.instance.playerLevel;
        UnitXPValue = CalculateUnitXPValue(PlayerManager.instance.LevelInstance);
        if(XPValue != null)
        {
            XPValue.SetSliderValue(UnitXPValue);
        }     
        Debug.Log($"Unit XP Value: {UnitXPValue}");      
        previousTokenCount = tokenCount;
        
        if (rotationCenter == null)
        {           
            Debug.LogWarning("Rotation center not assigned!");
        }
    }

    void Update()
    {
        if (inGlowSelection)
        {
            currentGlow = PlayerManager.instance.glowNumber;          
            HandleGlow();
        }        
        
        UpdateTokenCount();      
        OrbitAround(); // Orbit around the rotation center
        CorrectDistance(); // Adjust the distance to the desired distance
        HandleTokenCollision(); // Handle token collision
        CheckGameOver(); // Check if the player has multiple tokens in the scene

        if (!isMenu)
        {
            UpdateBackgroundColor();
            ManageZoomCoroutine();
            UpdateTokenCounterColor();

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
           
            if (rotationCenter == null)
            {
                return;
            }
        }

        triangleOne.clockwise = triangleTwo.clockwise = triangleThree.clockwise = triangleFour.clockwise = clockwise;
    }

    // Method to update the background color based on the token count
    void UpdateBackgroundColor()
    {
        if (!isMenu)
        {
            pastThirty = tokenCount > 30 ? true : false;
            pastSixty = tokenCount > 60 ? true : false;
            pastNinety = tokenCount > 90 ? true : false;
        }


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
        else if (pastNinety)
        {
            if (cam != null)
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

    // Method to change the trail color using the color gradient
    void ChangeTrailColorUsingGradient(Color color, float alpha)
    {
        if (trailRenderer != null)
        {
            // Create a new gradient
            Gradient gradient = new Gradient();

            // Define the gradient color keys
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0].color = color;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = color;
            colorKeys[1].time = 1.0f;

            // Define the gradient alpha keys
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0].alpha = alpha;
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = alpha;
            alphaKeys[1].time = 1.0f;

            // Assign the color and alpha keys to the gradient
            gradient.SetKeys(colorKeys, alphaKeys);

            // Set the trail renderer's color gradient to the new gradient
            trailRenderer.colorGradient = gradient;
        }
        else
        {
            Debug.LogWarning("TrailRenderer is not assigned.");
        }
    }
    void HandleGlow()
    {       

        // Destroy any existing glow instance if it doesn't match the current glow number
        if (glowInstance != null)
        {
            // Check if we need to destroy the existing instance
            bool shouldDestroy = false;
            if (currentGlow == 1 && glowInstance.name != SLOT1.name + "(Clone)")
            {
                shouldDestroy = true;
            }
            else if (currentGlow == 2 && glowInstance.name != SLOT2.name + "(Clone)")
            {
                shouldDestroy = true;
            }
            else if (currentGlow == 3 && glowInstance.name != SLOT3.name + "(Clone)")
            {
                shouldDestroy = true;
            }
            else if (currentGlow == 4 && glowInstance.name != SLOT4.name + "(Clone)")
            {
                shouldDestroy = true;
            }
            else if(currentGlow == 5 && glowInstance.name != SLOT5.name + "(Clone)")
            {
                shouldDestroy = true;
            }
            else if (currentGlow != 1 && currentGlow != 2 && currentGlow != 3 && currentGlow != 4 && currentGlow != 5)
            {
                shouldDestroy = true;
            }

            if (shouldDestroy)
            {
                // Destroy the existing glow instance
                Destroy(glowInstance);
                glowInstance = null;
                ChangeTrailColorUsingGradient(Color.black, 1);
            }
        }

        // Instantiate the new glow instance if required
        if (currentGlow == 1 && glowInstance == null)
        {
            if (SLOT1 != null)
            {
                glowInstance = Instantiate(SLOT1, transform.position, Quaternion.identity, transform);               
                ChangeTrailColorUsingGradient(Color.black, 1);
            }
            else
            {
                Debug.LogWarning("GlowPrefab SLOT1 not assigned!");
            }
        }
        else if (currentGlow == 2 && glowInstance == null)
        {
            if (SLOT2 != null)
            {
                glowInstance = Instantiate(SLOT2, transform.position, Quaternion.identity, transform);
                // Example of changing trail color for SLOT2
                ChangeTrailColorUsingGradient(new Color(1f, 1f, 0f), 0.5f);
            }
            else
            {
                Debug.LogWarning("GlowPrefab SLOT2 not assigned!");
            }
        }
        else if (currentGlow == 3 && glowInstance == null)
        {
            if (SLOT3 != null)
            {
                glowInstance = Instantiate(SLOT3, transform.position, Quaternion.identity, transform);                             
                ChangeTrailColorUsingGradient(Color.blue, 0.8f); // Adjust parameters as needed
            }
            else
            {
                Debug.LogWarning("GlowPrefab SLOT3 not assigned!");
            }
        }
        else if(currentGlow == 4 && glowInstance == null)
        {
            if(SLOT4 != null)
            {
                glowInstance = Instantiate(SLOT4, transform.position, Quaternion.identity, transform);              
                ChangeTrailColorUsingGradient(new Color(202, 0, 255), 0.8f);
            }
            else
            {
                Debug.LogWarning("GlowPrefab SLOT4 not assigned!");
            }
        }
        else if(currentGlow == 5 && glowInstance == null)
        {
            if(SLOT5 != null)
            {
                glowInstance = Instantiate(SLOT5, transform.position, Quaternion.identity, transform);              
                ChangeTrailColorUsingGradient(Color.black, 0f);
            }
            else
            {
                Debug.LogWarning("GlowPrefab SLOT5 not assigned!");
            }
        }
    }


    float CalculateUnitXPValue(int level)
    {
        float initialXPValue = 1f;
        float reductionFactor = 0.95f;

        // Calculate the UnitXPValue using the exponential formula
        float xpValue = initialXPValue * Mathf.Pow(reductionFactor, level - 1);

        // Return the calculated XP value
        return xpValue;
    }  

    void UpdateTokenCounterColor()
    {
        if (isCollidingWithHoldToken)
        {
            //Debug.Log("Colliding with hold token");
            tokenCounter.ChangeColor(new Color(0.5f, 0, 0.5f));
        }
        else if (isCollidingWithRedirectToken)
        {
            //Debug.Log("Colliding with redirect token");
            tokenCounter.ChangeColor(Color.blue);
        }
        else if (isCollidingWithRedToken)
        {
            //Debug.Log("Colliding with red token");
            tokenCounter.ChangeColor(Color.red);
        }
        else if (isCollidingWithToken)
        {
         
            tokenCounter.ChangeColor(new Color(1.0f, 0.92f, 0.3f));
        }
    }


    

    // Method to manage the zooming coroutine
    void ManageZoomCoroutine()
    {
        if (isCollidingWithHoldToken && zoomCoroutine == null && Input.touchCount > 0 && !dead)
        {           
            AudioManager.Instance.PlaySFX(AudioManager.Instance.standardClick);
            
            // Start the zooming coroutine if it's not already running
            zoomCoroutine = StartCoroutine(ContinuousZoomInAndBack());          
           ChangeTrailColorUsingGradient(trailRenderer.colorGradient.colorKeys[0].color, 0);
        }
        else if (!isCollidingWithHoldToken && zoomCoroutine != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.standardClick);
            // Don't stop the coroutine immediately; it will handle zooming out by itself
            zoomCoroutine = null;
            if(currentGlow != 5)
            {
                ChangeTrailColorUsingGradient(trailRenderer.colorGradient.colorKeys[0].color, 1);
            }          
        }
    }

    // Method to update the token count and adjust the rotate speed
    void UpdateTokenCount()
    {
        if (tokenCount != previousTokenCount && rotateSpeed < 350 && !isMenu)
        {           
            rotateSpeed = initialRotateSpeed + tokenCount;
            //Debug.Log($"Current Speed: {rotateSpeed}");

            // Update previousTokenCount to current tokenCount
            previousTokenCount = tokenCount;
        }
    }

    // Method to handle token collision
    void HandleTokenCollision()
    {
        if ((isCollidingWithToken || isCollidingWithRedirectToken || isCollidingWithRedToken) && Input.touchCount > 0 && !isMenu && !dead)
        {                                
            if (isCollidingWithToken)
            {
                goldTokenCount++;
                PlayerManager.instance.AddGoldTokens(1);
                PlayerManager.instance.AddXP(UnitXPValue);
            }
            if(isCollidingWithRedirectToken)
            {
                blueTokenCount++;
                PlayerManager.instance.AddBlueTokens(1);
                PlayerManager.instance.AddXP(UnitXPValue * 5);
            }   
            if(isCollidingWithRedToken)
            {
                redTokenCount++;
                PlayerManager.instance.AddXP(UnitXPValue * 25);
            }
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
        if (Time.time - lastTokenDestructionTime > gracePeriod && CountTokens() > 1 && !isMenu)
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
            if (isMenu) { rotationCenterScript.SimulatePressWithDelay(); } // Simulate the press on the rotation center
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
            purpleTokenCount++;
            PlayerManager.instance.AddPurpleTokens(1);
            PlayerManager.instance.AddXP(UnitXPValue * 10);
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
            if (Input.touchCount == 0 && !isMenu && !dead)
            {
                Die();
            }
            // Reset the collision flag and reference
            isCollidingWithToken = false;
            currentToken = null;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {           
            if (Input.touchCount == 0 && !dead)
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
            if (Input.touchCount == 0 && !dead)
            {
                Die();
            }

            isCollidingWithRedToken = false;
            currentToken = null;
            if (!isMenu) { RotateCamera(); }
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
        while (elapsedTime < flashDuration && !isMenu)
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

        while (elapsedTime < zoomDuration && !isMenu)
        {
            cam.orthographicSize = Mathf.Lerp(targetSize, originalSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera size is exactly the original at the end
        if (!isMenu) { cam.orthographicSize = originalSize; }
    }

    // Coroutine to continuously zoom in and back when colliding with a hold token
    IEnumerator ContinuousZoomInAndBack()
    {
        if (cam == null) yield break;

        float zoomFactor = 0.95f; // Zoom in factor per frame
        float maxZoomFactor = 0.9f; // Maximum zoom limit (50% of original size)
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
    public void Die()
    {
        AudioManager.Instance.SFX.volume = 0;
        DestroyAllTokens(); // Destroy all tokens in the scene
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        // Call the ScaleOverTime coroutine and wait for it to complete
        yield return StartCoroutine(ScaleOverTime(Vector3.zero, 0.5f)); // Adjust duration as needed

        // Now execute the remaining logic
        goldTokenCurrent.UpdateCurrentGoldToken(goldTokenCount);
        blueTokenCurrent.UpdateCurrentBlueToken(blueTokenCount);
        redTokenCurrent.UpdateCurrentRedToken(redTokenCount);
        purpleTokenCurrent.UpdateCurrentPurpleToken(purpleTokenCount);

        // Deactivate the player and its children
        gameObject.SetActive(false);
        dead = true; // Set the dead flag to true       

        // Optionally, you could trigger any other death-related logic here, like fading out
        ease.FadeIn();
        if (!isMenu) { tokenCounter.textMeshPro.color = Color.white; }
    }

    // Method to handle token destruction (unchanged)
    public void DestroyCurrentToken()
    {
        Destroy(currentToken);
    }

    private IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void DestroyAllTokens()
    {
        // Find all game objects with the tag "Token"
        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        // Find all game objects with the tag "RedirectToken"
        GameObject[] redirectTokens = GameObject.FindGameObjectsWithTag("RedirectToken");

        GameObject[] holdTokens = GameObject.FindGameObjectsWithTag("HoldToken");

        GameObject[] redTokens = GameObject.FindGameObjectsWithTag("RedToken");

        // Iterate through the array and destroy each game object tagged "Token"
        foreach (GameObject token in tokens)
        {
            Destroy(token);
        }
        // Iterate through the array and destroy each game object tagged "RedirectToken"
        foreach (GameObject redirectToken in redirectTokens)
        {
            Destroy(redirectToken);
        }
        foreach (GameObject holdToken in holdTokens)
        {
            Destroy(holdToken);
        }
        foreach (GameObject redToken in redTokens)
        {
            Destroy(redToken);
        }
    }


}
