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

    // Start is called before the first frame update
    void Start()
    {
        // Try to get both components
        graphicComponent = GetComponent<Graphic>();
        spriteRendererComponent = GetComponent<SpriteRenderer>();

        // Check if at least one component is found
        if (graphicComponent == null && spriteRendererComponent == null)
        {
            Debug.LogError("No Graphic or SpriteRenderer component found on the GameObject.");
            return;
        }

        // Define the list of colors
        colors = new List<Color>()
        {           
            new Color(1.0f * factor, 0.0f * factor, 0.0f * factor),
            new Color(0.0f * factor, 0.0f * factor, 1.0f * factor),
            new Color(0.5f * factor, 0.0f * factor, 0.5f * factor),
            new Color(1.0f * factor, 0.92f * factor, 0.3f * factor)
        };

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

    // Update is called once per frame
    void Update()
    {
        // No need to update every frame if only color changing is handled in coroutine
    }

    private IEnumerator CycleColors()
    {
        int currentColorIndex = 0;
        while (true)
        {
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
}
