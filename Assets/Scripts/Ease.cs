using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Ease : MonoBehaviour
{
    private float duration = 1.0f;  // Duration of the fade
    private Renderer objectRenderer;
    private bool canFade = true; // Flag to check if the object can fade

    void Start()
    {       
        // Get the Renderer component from the GameObject
        objectRenderer = GetComponent<Renderer>();

        // Start fading out the object over the specified duration
        StartCoroutine(FadeOut(duration));
    }  
    //update is called once per frame
    void Update()
    {       
    }
    IEnumerator FadeOut(float duration)
    {
        // Get the current color of the material
        Color startColor = objectRenderer.material.color;

        // Iterate over the duration
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // Calculate the normalized time (0 to 1)
            float normalizedTime = t / duration;

            // Interpolate the alpha value
            float alpha = Mathf.Lerp(1, 0, normalizedTime);

            // Apply the new color with the interpolated alpha
            objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha is set to 0
        objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
    //fade in
    IEnumerator FadeIn(float duration)
    {
        // Get the current color of the material
        Color startColor = objectRenderer.material.color;

        // Iterate over the duration
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // Calculate the normalized time (0 to 1)
            float normalizedTime = t / duration;

            // Interpolate the alpha value
            float alpha = Mathf.Lerp(0, 1, normalizedTime);

            // Apply the new color with the interpolated alpha
            objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha is set to 1
        objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, 1);        
    }

    public void FadeIn()
    {
        if (canFade)
        {
            canFade = false;
            StartCoroutine(FadeIn(duration));
        }
                     
    }
}
