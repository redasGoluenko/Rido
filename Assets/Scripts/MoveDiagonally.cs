using UnityEngine;
using System.Collections;

public class MoveDiagonally : MonoBehaviour
{
    // Adjust this value to change the speed of the movement
    public float speed = 2.0f;

    // Adjust this value to change the distance of movement
    public float distance = 100.0f;

    public float delay = 1.0f; // Adjust delay time in seconds
    public bool moveVertically = false; // Set to true to move vertically instead of diagonally

    private RectTransform rectTransform;
    private Transform objectTransform;
    private Vector2 startPos;
    private Vector3 startPosition;

    void Start()
    {
        StartCoroutine(StartMovementWithDelay());
    }

    IEnumerator StartMovementWithDelay()
    {
        yield return new WaitForSeconds(delay);

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
        if (moveVertically)
        {
            StartCoroutine(MoveVerticallyCoroutine());
        }
        else
        {
            StartCoroutine(MoveDiagonallyCoroutine());
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

    //move vertically coroutine
    IEnumerator MoveVerticallyCoroutine()
    {
        while (true)
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
