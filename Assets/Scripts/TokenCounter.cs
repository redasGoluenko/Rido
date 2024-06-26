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
            textMeshPro.fontSize = 200; // Increase the font size
            if (rotate.pastNinety)
            {
                textMeshPro.color = Color.red;
            }
            else if (rotate.pastSixty)
            {
                textMeshPro.color = new Color(0.5f, 0, 0.5f);
            }
            else if (rotate.pastThirty)
            {
                textMeshPro.color = Color.blue;
            }
            else
            {
                textMeshPro.color = Color.yellow;
            }

            // Stop any existing flashing coroutine
            if (flashingCoroutine != null)
            {
                StopCoroutine(flashingCoroutine);
                flashingCoroutine = null;
            }
        }     

        // Update the text to display the current token count
        textMeshPro.text = rotate.tokenCount.ToString();
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
