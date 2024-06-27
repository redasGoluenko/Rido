using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components

public class CycleLiningColors : MonoBehaviour
{
    private List<Color> colors; // List to hold the pastel colors
    private Graphic graphicComponent; // Reference to the Graphic component (like Image or Text)
    private Coroutine colorCoroutine; // Reference to the color change coroutine

    public float colorChangeInterval = 2.0f; // Time between color changes in seconds
    private float factor = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        graphicComponent = GetComponent<Graphic>(); // Get the Graphic component
        if (graphicComponent == null)
        {
            Debug.LogError("No Graphic component found on the GameObject.");
            return;
        }

        colors = new List<Color>()
        {
            new Color(1.0f * factor, 0.92f * factor, 0.3f * factor),
            new Color(1.0f * factor, 0.0f * factor, 0.0f * factor),
            new Color(0.0f * factor, 0.0f * factor, 1.0f * factor),
            new Color(0.5f * factor, 0.0f * factor, 0.5f * factor)
        };

        // Start with alpha 0
        graphicComponent.color = new Color(colors[0].r, colors[0].g, colors[0].b, graphicComponent.color.a);

        // Start the color changing coroutine
        colorCoroutine = StartCoroutine(CycleColors());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator CycleColors()
    {
        int currentColorIndex = 0;
        while (true)
        {
            // Get the next color in the list
            Color nextColor = colors[(currentColorIndex + 1) % colors.Count];
            float time = 0;
            Color startColor = graphicComponent.color;

            while (time < colorChangeInterval)
            {
                time += Time.deltaTime;
                float t = time / colorChangeInterval;

                // Interpolate between the current color (keeping alpha) and the next color (without changing alpha)
                graphicComponent.color = new Color(
                    Mathf.Lerp(startColor.r, nextColor.r, t),
                    Mathf.Lerp(startColor.g, nextColor.g, t),
                    Mathf.Lerp(startColor.b, nextColor.b, t),
                    graphicComponent.color.a // keep the current alpha
                );

                yield return null;
            }

            // Move to the next color in the list
            currentColorIndex = (currentColorIndex + 1) % colors.Count;
        }
    }
}
