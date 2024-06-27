using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyVisiblePivot : MonoBehaviour
{
    private Coroutine fadeCoroutine; // Reference to the fade coroutine
    private Coroutine colorCoroutine; // Reference to the color change coroutine
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public float fadeDuration = 0.5f;  // Duration of the fade in seconds (adjust to a reasonable value for smooth fade)
    public float colorChangeInterval = 2.0f; // Time between color changes in seconds
    private float factor = 0.90f; // Factor to adjust the pastel colors
    public bool isMenu = false;

    private List<Color> colors; // List to hold the pastel colors

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        spriteRenderer.enabled = true;  // SpriteRenderer needs to be enabled to control its color

        if (isMenu) { HandlePivotColors(); }
        else
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }
    void HandlePivotColors()
    {
        // Initialize colors
        colors = new List<Color>()
        {           
            new Color(1f * factor, 0.6f * factor, 0.6f * factor, spriteRenderer.color.a), // Pastel Red
            new Color(0.7f * factor, 0.85f * factor, 1f * factor, spriteRenderer.color.a), // Pastel Blue
            new Color(0.85f * factor, 0.7f * factor, 1f * factor, spriteRenderer.color.a),  // Pastel Purple
            new Color(1f * factor, 0.96f * factor, 0.7f * factor, spriteRenderer.color.a) // Pastel Yellow
        };

        // Start with alpha 0
        spriteRenderer.color = new Color(colors[0].r, colors[0].g, colors[0].b, 0);

        // Start the color changing coroutine
        colorCoroutine = StartCoroutine(CycleColors());
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

    // Coroutine to cycle through colors
    private IEnumerator CycleColors()
    {
        int currentColorIndex = 0;
        while (true)
        {
            // Get the next color in the list
            Color nextColor = colors[(currentColorIndex + 1) % colors.Count];
            float time = 0;
            Color startColor = spriteRenderer.color;

            while (time < colorChangeInterval)
            {
                time += Time.deltaTime;
                float t = time / colorChangeInterval;

                // Interpolate between the current color (keeping alpha) and the next color (without changing alpha)
                spriteRenderer.color = new Color(
                    Mathf.Lerp(startColor.r, nextColor.r, t),
                    Mathf.Lerp(startColor.g, nextColor.g, t),
                    Mathf.Lerp(startColor.b, nextColor.b, t),
                    spriteRenderer.color.a // keep the current alpha
                );

                yield return null;
            }

            // Move to the next color in the list
            currentColorIndex = (currentColorIndex + 1) % colors.Count;
        }
    }
}
