using UnityEngine;
using System.Collections;

public class MoveDiagonally : MonoBehaviour
{
    // Adjust this value to change the speed of the movement
    public float speed = 2.0f;

    // Adjust this value to change the distance of movement
    public float distance = 100.0f;

    public float delay = 1.0f; // Adjust delay time in seconds

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        StartCoroutine(StartMovementWithDelay());
    }

    IEnumerator StartMovementWithDelay()
    {
        yield return new WaitForSeconds(delay);

        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;

        // Start moving after the delay
        StartCoroutine(MoveDiagonallyCoroutine());
    }

    IEnumerator MoveDiagonallyCoroutine()
    {
        while (true)
        {
            // Calculate horizontal and vertical offsets based on angle and time
            float xOffset = Mathf.Cos(Mathf.Deg2Rad * 22.23f) * Mathf.Sin(Time.time * speed) * distance;
            float yOffset = Mathf.Sin(Mathf.Deg2Rad * 22.23f) * Mathf.Sin(Time.time * speed) * distance;

            // Apply the offsets to the anchored position
            rectTransform.anchoredPosition = startPos + new Vector2(xOffset, yOffset);

            yield return null; // Wait until the next frame
        }
    }
}
