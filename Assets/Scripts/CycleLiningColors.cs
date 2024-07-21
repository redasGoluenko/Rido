using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components

public class CycleLiningColors : MonoBehaviour
{
    private List<Color> colors; // List to hold the pastel colors
    private Graphic graphicComponent; // Reference to the Graphic component (like Image or Text)
    private SpriteRenderer spriteRendererComponent; // Reference to the SpriteRenderer component
    private Coroutine colorCoroutine; // Reference to the color change coroutine

    public float colorChangeInterval = 2.0f; // Time between color changes in seconds
    private float factor = 0.3f;
    public bool pastel = false;
    public bool black = false;
    public bool pause = false; // Boolean flag to pause the cycling

    // Start is called before the first frame update
    void Start()
    {
        // Try to get both components
        graphicComponent = GetComponent<Graphic>();
        spriteRendererComponent = GetComponent<SpriteRenderer>();

        if (graphicComponent == null && spriteRendererComponent == null)
        {
            Debug.LogError("No Graphic or SpriteRenderer component found on the GameObject.");
            return;
        }

        if (!pastel)
        {
            // Define the list of colors
            colors = new List<Color>()
            {
                new Color(1.0f * factor, 0.0f * factor, 0.0f * factor),
                new Color(0.0f * factor, 0.0f * factor, 1.0f * factor),
                new Color(0.5f * factor, 0.0f * factor, 0.5f * factor),
                new Color(1.0f * factor, 0.92f * factor, 0.3f * factor)
            };
        }
        else
        {
            colors = new List<Color>()
            {
                new Color(1f, 0.6f, 0.6f),
                new Color(0.7f, 0.85f, 1f),
                new Color(0.85f, 0.7f, 1f),
                new Color(1f, 0.96f, 0.7f)
            };
        }

        // Start with the first color and maintain alpha if any component is present
        if (graphicComponent != null)
        {
            graphicComponent.color = new Color(colors[0].r, colors[0].g, colors[0].b, graphicComponent.color.a);
        }
        else if (spriteRendererComponent != null)
        {
            spriteRendererComponent.color = new Color(colors[0].r, colors[0].g, colors[0].b, spriteRendererComponent.color.a);
        }

        // Start the color changing coroutine
        colorCoroutine = StartCoroutine(CycleColors());
    }

    private IEnumerator CycleColors()
    {
        int currentColorIndex = 0;
        while (true)
        {
            // Check if paused
            if (pause)
            {
                yield return null; // Skip this frame and continue checking
                continue;
            }

            // Get the next color in the list
            Color nextColor = colors[(currentColorIndex + 1) % colors.Count];
            float time = 0;

            Color startColor = Color.black;

            if (graphicComponent != null)
            {
                startColor = graphicComponent.color;
            }
            else if (spriteRendererComponent != null)
            {
                startColor = spriteRendererComponent.color;
            }

            while (time < colorChangeInterval)
            {
                // Check if paused
                if (pause)
                {
                    yield return null; // Skip this frame and continue checking
                    continue;
                }

                time += Time.deltaTime;
                float t = time / colorChangeInterval;

                Color interpolatedColor = new Color(
                    Mathf.Lerp(startColor.r, nextColor.r, t),
                    Mathf.Lerp(startColor.g, nextColor.g, t),
                    Mathf.Lerp(startColor.b, nextColor.b, t),
                    startColor.a // keep the current alpha
                );

                if (graphicComponent != null)
                {
                    graphicComponent.color = interpolatedColor;
                }
                else if (spriteRendererComponent != null)
                {
                    spriteRendererComponent.color = interpolatedColor;
                }

                yield return null;
            }

            // Move to the next color in the list
            currentColorIndex = (currentColorIndex + 1) % colors.Count;
        }
    }

    // Method to pause the color cycling
    public void PauseCycling()
    {
        pause = true;
    }

    // Method to resume the color cycling
    public void ResumeCycling()
    {
        pause = false;
    }
    public void ChangeColor(Color targetColor)
    {
        if (graphicComponent != null)
        {
            StartCoroutine(FadeColor(graphicComponent, targetColor));
        }
        else if (spriteRendererComponent != null)
        {
            StartCoroutine(FadeColor(spriteRendererComponent, targetColor));
        }
    }

    // Coroutine to fade to the target color
    private IEnumerator FadeColor(Graphic graphic, Color targetColor)
    {
        Color startColor = graphic.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            graphic.color = Color.Lerp(startColor, targetColor, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        graphic.color = targetColor; // Ensure the final color is set
    }

    // Overloaded coroutine to fade to the target color for SpriteRenderer
    private IEnumerator FadeColor(SpriteRenderer spriteRenderer, Color targetColor)
    {
        Color startColor = spriteRenderer.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        spriteRenderer.color = targetColor; // Ensure the final color is set
    }
}
