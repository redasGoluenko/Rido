using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TokenCounter : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public TextMeshProUGUI textMeshPro; // Reference to the TextMeshProUGUI component  
    public RectTransform tokenCounterRectTransform; // Reference to the RectTransform of the token counter
    public Coroutine flashingCoroutine; // Reference to the coroutine for flashing   

    private bool hasMoved = false; // Flag to track if the token counter has already moved     
    void Start()
    {
        textMeshPro.color = Color.white; // Set the initial color to white
        // Get the RectTransform component of the token counter
        tokenCounterRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate.dead)
        {
            // If rotate.dead is true, set the color to white
            textMeshPro.color = Color.white;

            // Stop any existing flashing coroutine
            if (flashingCoroutine != null)
            {
                StopCoroutine(flashingCoroutine);
                flashingCoroutine = null;
            }
        }     

        if (!rotate.gameObject.activeSelf && !hasMoved)
        {
            //Debug.Log("Rotate object is inactive");
            // Move the button to the left by moveDistance units
            MoveCounter();
            hasMoved = true; // Set the flag to true to indicate movement
        }

        // Update the text to display the current token count
        textMeshPro.text = rotate.tokenCount.ToString();
    }
    
    // Method to move the token counter
    private void MoveCounter()
    {
        Vector3 newPosition = tokenCounterRectTransform.anchoredPosition;
        
        if(rotate.tokenCount > 99)
        {
            newPosition.x += 90f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 300;
        }
        else if(rotate.tokenCount > 9)
        {
            newPosition.x += 130f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 500;
        }
        else
        {
            newPosition.x += 185f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 700;
        }
        
        tokenCounterRectTransform.anchoredPosition = newPosition;
    }

    // Method to start the flashing color effect
    public void ChangeColor(Color targetColor, float flashDuration = 0.4f, int flashCount = 1)
    {
        // Stop any existing flashing coroutine
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
        }

        // Start the new flashing coroutine
        flashingCoroutine = StartCoroutine(FlashToColor(targetColor, flashDuration, flashCount));
    }

    // Coroutine to smoothly transition to the target color and stay on it
    private IEnumerator FlashToColor(Color targetColor, float flashDuration, int flashCount)
    {
        Color originalColor = Color.white; // Store the original color
        float halfFlashDuration = flashDuration / (2 * flashCount); // Duration for each half of the flash (ease-in and ease-out)

        // Smoothly transition to the target color
        float elapsedTime = 0f;
        while (elapsedTime < halfFlashDuration)
        {
            textMeshPro.color = Color.Lerp(originalColor, targetColor, elapsedTime / halfFlashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = targetColor; // Ensure it's set to the target color

        // Stay on the target color until further interaction
        while (Input.touchCount > 0) // Change condition based on your input method
        {
            yield return null; // Wait until interaction is released
        }

        // Smoothly transition back to the original color
        elapsedTime = 0f;
        while (elapsedTime < halfFlashDuration)
        {
            textMeshPro.color = Color.Lerp(targetColor, originalColor, elapsedTime / halfFlashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMeshPro.color = originalColor; // Ensure it's set to the original color

        // Clear the coroutine reference
        flashingCoroutine = null;
    }
}
