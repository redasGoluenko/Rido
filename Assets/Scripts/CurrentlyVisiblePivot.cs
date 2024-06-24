using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyVisiblePivot : MonoBehaviour
{
    private Coroutine fadeCoroutine; // Reference to the fade coroutine
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public float fadeDuration = 0.0001f;  // Duration of the fade in seconds
    
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        spriteRenderer.enabled = true;  // SpriteRenderer needs to be enabled to control its color
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0); // Set the alpha to 0
    }

    // When the object collides with the sight object
    private void OnCollisionEnter2D(Collision2D collision)
    {      
        if (collision.gameObject.CompareTag("Sight"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeToAlpha(1.0f));
        }
    }

    // When the object exits the sight object
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sight"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeToAlpha(0.0f));
        }
    }

    // Coroutine to fade the alpha of the SpriteRenderer
    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        // Ensure the target alpha is set at the end
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
    }
}
