using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotationCenter; // Reference to the object we want to rotate around
    private float rotateSpeed = 100f; // Speed of rotation in degrees per second
    public bool clockwise = true; // Direction of rotation
    public float desiredDistance = 5f; // The desired distance from the rotation center
    public float correctionSpeed = 2f; // Speed at which the distance correction happens
    public bool isCollidingWithToken = false; // Flag to track collision with objects tagged as "Token"
    private GameObject currentToken; // Reference to the currently collided token
    private float lastTokenDestructionTime = -1f; // Time when the last token was destroyed
    public float gracePeriod = 0.5f; // Grace period in seconds to ignore brief multiple token situations
    public int tokenCount = 0; // Number of tokens picked up by the player
    public Ease ease; // Reference to the Ease script
    public Camera cam;

    private void Start()
    {      
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
        //increase rotate speed on screen press
        if (Input.touchCount > 0)
        {
            rotateSpeed += 110f * Time.deltaTime; // Adjust the multiplier as needed
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
        if (isCollidingWithToken && Input.touchCount > 0)
        {
            // Destroy the token
            Destroy(currentToken);
            StartCoroutine(FlashBackground());
            tokenCount++;
            // Record the time of token destruction
            lastTokenDestructionTime = Time.time;
            // Reset the flag and reference after destroying the token
            isCollidingWithToken = false;
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
            isCollidingWithToken = true; // Flag to track collision state
            currentToken = collision.gameObject; // Store the reference to the collided token
        }
    }

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
    }
    IEnumerator FlashBackground()
    {
        if (cam == null) yield break;

        Color flashColor = new Color(0.8f, 0.8f, 0.8f, 1f);     
        float flashDuration = 0.2f; // The duration of the flash effect
        float elapsedTime = 0f;

        // Change background to flash color
        cam.backgroundColor = flashColor;

        // Smoothly interpolate back to the original color
        while (elapsedTime < flashDuration)
        {
            cam.backgroundColor = Color.Lerp(flashColor, Color.grey, elapsedTime / flashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the background color is exactly the original at the end
        cam.backgroundColor = Color.grey;
    }


    // Method to count the number of active tokens in the scene
    int CountTokens()
    {
        // Find all GameObjects tagged as "Token"
        GameObject[] tokens = GameObject.FindGameObjectsWithTag("Token");
        // Return the count of these objects
        return tokens.Length;
    }
    public void Die()
    {       
        gameObject.SetActive(false);       
            ease.FadeIn();             
    }
}
