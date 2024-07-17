using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveDiagonally : MonoBehaviour
{
    // Adjust this value to change the speed of the movement
    public float speed = 2.0f;

    // Adjust this value to change the distance of movement
    public float distance = 100.0f;

    public float delay = 1.0f; // Adjust delay time in seconds
    public bool moveVertically = false; // Set to true to move vertically instead of diagonally
    public float initialRightDistance = 200.0f; // Distance to move diagonally initially
    public bool slopeMovement = true;

    private RectTransform rectTransform;
    private Transform objectTransform;
    private Vector2 startPos;
    private Vector3 startPosition;

    private bool inGlowSelection;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Glows")
        {           
            inGlowSelection = true;                 
        }
        else
        {
            inGlowSelection = false;
        }

        StartCoroutine(StartMovementWithDelay());
    }

    IEnumerator StartMovementWithDelay()
    {
        yield return new WaitForSeconds(delay);

        
            StartCoroutine(MoveVerticallyCoroutine());

            // Check if the object has a RectTransform or a regular Transform
            rectTransform = GetComponent<RectTransform>();
            objectTransform = GetComponent<Transform>();

            if (rectTransform != null)
            {
                startPos = rectTransform.anchoredPosition;
            }
            else if (objectTransform != null)
            {
                startPosition = objectTransform.position;
            }       
        if(!moveVertically)
        {          
            // Start initial diagonal right movement
            yield return StartCoroutine(MoveRightDiagonallyInitiallyCoroutine());
            StartCoroutine(MoveDiagonallyCoroutine());
        }
        
    }

    // Coroutine for initial diagonal right movement
    IEnumerator MoveRightDiagonallyInitiallyCoroutine()
    {
        if (!inGlowSelection)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.whoosh);
        }      
        yield return new WaitForSeconds(0.25f);
        float initialSpeed = speed * 10000; // Speed for the initial diagonal movement
        float initialTime = initialRightDistance / initialSpeed; // Time to complete the initial movement

        float elapsedTime = 0f;

        while (elapsedTime < initialTime)
        {
            // Calculate offsets for diagonal movement
            float xOffset = Mathf.Cos(Mathf.Deg2Rad * 22.23f) * Mathf.Lerp(0, initialRightDistance, elapsedTime / initialTime);
            float yOffset = Mathf.Sin(Mathf.Deg2Rad * 22.23f) * Mathf.Lerp(0, initialRightDistance, elapsedTime / initialTime);

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = startPos + new Vector2(xOffset, yOffset);
            }
            else if (objectTransform != null)
            {
                objectTransform.position = startPosition + new Vector3(xOffset, yOffset, 0);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the position is set to the final target position after the loop
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = startPos + new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * 22.23f) * initialRightDistance,
                Mathf.Sin(Mathf.Deg2Rad * 22.23f) * initialRightDistance);
        }
        else if (objectTransform != null)
        {
            objectTransform.position = startPosition + new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * 22.23f) * initialRightDistance,
                Mathf.Sin(Mathf.Deg2Rad * 22.23f) * initialRightDistance, 0);
        }

        // Update the starting position for subsequent movement
        if (rectTransform != null)
        {
            startPos = rectTransform.anchoredPosition;
        }
        else if (objectTransform != null)
        {
            startPosition = objectTransform.position;
        }     
    }

    public IEnumerator MoveLeftDiagonallyInitiallyCoroutine()
    {
        if (!inGlowSelection)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.whoosh);
        }
        yield return new WaitForSeconds(0.25f);
        float initialSpeed = speed * 10000; // Speed for the initial diagonal movement
        float initialTime = initialRightDistance / initialSpeed; // Time to complete the initial movement

        float elapsedTime = 0f;

        while (elapsedTime < initialTime)
        {
            // Calculate offsets for diagonal movement to the left
            float xOffset = -Mathf.Cos(Mathf.Deg2Rad * 22.23f) * Mathf.Lerp(0, initialRightDistance, elapsedTime / initialTime);
            float yOffset = -Mathf.Sin(Mathf.Deg2Rad * 22.23f) * Mathf.Lerp(0, initialRightDistance, elapsedTime / initialTime);

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = startPos + new Vector2(xOffset, yOffset);
            }
            else if (objectTransform != null)
            {
                objectTransform.position = startPosition + new Vector3(xOffset, yOffset, 0);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the position is set to the final target position after the loop
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = startPos + new Vector2(
                -Mathf.Cos(Mathf.Deg2Rad * 22.23f) * initialRightDistance,
                -Mathf.Sin(Mathf.Deg2Rad * 22.23f) * initialRightDistance);
        }
        else if (objectTransform != null)
        {
            objectTransform.position = startPosition + new Vector3(
                -Mathf.Cos(Mathf.Deg2Rad * 22.23f) * initialRightDistance,
                -Mathf.Sin(Mathf.Deg2Rad * 22.23f) * initialRightDistance, 0);
        }

        // Update the starting position for subsequent movement
        if (rectTransform != null)
        {
            startPos = rectTransform.anchoredPosition;
        }
        else if (objectTransform != null)
        {
            startPosition = objectTransform.position;
        }
    }




    IEnumerator MoveDiagonallyCoroutine()
    {       
        while (true)
        {
            // Calculate horizontal and vertical offsets based on angle and time
            float xOffset = Mathf.Cos(Mathf.Deg2Rad * 22.23f) * Mathf.Sin(Time.time * speed) * distance;
            float yOffset = Mathf.Sin(Mathf.Deg2Rad * 22.23f) * Mathf.Sin(Time.time * speed) * distance;

            if (rectTransform != null)
            {
                // Apply the offsets to the anchored position for UI elements
                rectTransform.anchoredPosition = startPos + new Vector2(xOffset, yOffset);
            }
            else if (objectTransform != null)
            {
                // Apply the offsets to the position for regular game objects
                objectTransform.position = startPosition + new Vector3(xOffset, yOffset, 0);
            }

            yield return null; // Wait until the next frame
        }
    }

    public IEnumerator MoveVerticallyCoroutine()
    {     
        while (slopeMovement)
        {
            // Calculate vertical offset based on time
            float yOffset = Mathf.Sin(Time.time * speed) * distance;

            if (rectTransform != null)
            {
                // Apply the offset to the anchored position for UI elements
                rectTransform.anchoredPosition = startPos + new Vector2(0, yOffset);
            }
            else if (objectTransform != null)
            {
                // Apply the offset to the position for regular game objects
                objectTransform.position = startPosition + new Vector3(0, yOffset, 0);
            }

            yield return null; // Wait until the next frame
        }
    }
}
