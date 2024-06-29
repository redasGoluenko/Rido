using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyVisiblePivot : MonoBehaviour
{
    private Coroutine fadeCoroutine; // Reference to the fade coroutine
    private Coroutine colorCoroutine; // Reference to the color change coroutine
    private Coroutine scaleCoroutine; // Reference to the scaling coroutine
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private SpriteRenderer triangleSpriteRenderer; // Reference to the SpriteRenderer component of the triangle
    public GameObject player; // Reference to the player GameObject
    public GameObject trianglePrefab; // Reference to the triangle prefab

    public float fadeDuration = 0.5f;  // Duration of the fade in seconds
    public float colorChangeInterval = 2.0f; // Time between color changes in seconds
    public float pulsateDuration = 1.0f; // Duration of one pulsate cycle in seconds
    public float extensionOffset = 0.3f; // Offset to extend the triangle to the right
    private float factor = 0.90f; // Factor to adjust the pastel colors

    public Vector3 minScale = new Vector3(0.9f, 0.9f, 0.9f); // Minimum scale for the pulsation
    public Vector3 maxScale = new Vector3(1.1f, 1.1f, 1.1f); // Maximum scale for the pulsation    
    public bool isMenu = false;
    public bool isFirstPivot = false;
    

    private List<Color> colors; // List to hold the pastel colors

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        spriteRenderer.enabled = true;  // SpriteRenderer needs to be enabled to control its color

        // Spawn and configure the triangle
        SpawnTriangleToTheRight();

        if (isMenu)
        {
            HandlePivotColors();
        }
        else
        {
            if (isFirstPivot)
            {
                // Start with fully opaque color for the first pivot
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
                if (triangleSpriteRenderer != null)
                {
                    triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, 1);
                }
            }
            else
            {
                // Start with fully transparent color for other pivots
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
                if (triangleSpriteRenderer != null)
                {
                    triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, 0);
                }
            }
        }

        // Start the pulsating coroutine
        scaleCoroutine = StartCoroutine(Pulsate());
    }


    void Update()
    {
        // Always face the player if the player reference is set
        if (player != null)
        {
            FacePlayer();
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

        // Start with the first color and set alpha to 0
        spriteRenderer.color = new Color(colors[0].r, colors[0].g, colors[0].b, 0);
        if (triangleSpriteRenderer != null)
        {
            triangleSpriteRenderer.color = new Color(colors[0].r, colors[0].g, colors[0].b, 0);
        }

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
        if(collision.gameObject.CompareTag("RotationCenter"))
        {
            //ChangeTriangleAlphaToOne();
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
        if (collision.gameObject.CompareTag("RotationCenter"))
        {
            //ChangeTriangleAlphaToZero();
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
            if (triangleSpriteRenderer != null)
            {
                triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, alpha);
            }
            yield return null;
        }

        // Ensure the target alpha is set at the end
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
        if (triangleSpriteRenderer != null)
        {
            triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, targetAlpha);
        }
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

                if (triangleSpriteRenderer != null)
                {
                    triangleSpriteRenderer.color = new Color(
                        Mathf.Lerp(startColor.r, nextColor.r, t),
                        Mathf.Lerp(startColor.g, nextColor.g, t),
                        Mathf.Lerp(startColor.b, nextColor.b, t),
                        triangleSpriteRenderer.color.a // keep the current alpha
                    );
                }

                yield return null;
            }

            // Move to the next color in the list
            currentColorIndex = (currentColorIndex + 1) % colors.Count;
        }
    }

    // Coroutine to pulsate the object's scale
    private IEnumerator Pulsate()
    {
        while (true)
        {
            // Pulsate to max scale
            yield return ScaleOverTime(minScale, maxScale, pulsateDuration / 2);
            // Pulsate to min scale
            yield return ScaleOverTime(maxScale, minScale, pulsateDuration / 2);
        }
    }

    // Coroutine to scale the object over a duration
    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            yield return null;
        }
        transform.localScale = endScale;
    }

    // Method to make the object face the player
    private void FacePlayer()
    {
        // Calculate the direction vector from the object to the player
        Vector2 direction = player.transform.position - transform.position;

        // Normalize the direction vector to get the direction
        direction.Normalize();

        // Calculate the rotation angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the object (for 2D, we only modify the z-axis rotation)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Method to spawn the triangle to the right of the object
    private void SpawnTriangleToTheRight()
    {
        if (trianglePrefab != null)
        {
            // Instantiate the triangle prefab
            GameObject triangle = Instantiate(trianglePrefab);

            // Calculate the position to the right of the object
            // Adding half the width of the object to the right (x-axis) of the object
            float offsetX = (spriteRenderer.bounds.size.x / 2) + extensionOffset;

            // Set the triangle's position to be to the right of the object's center
            triangle.transform.position = transform.position + new Vector3(offsetX, 0, 0);

            // Parent the triangle to the object to follow its movement and rotation
            triangle.transform.parent = transform;

            // Adjust the local position to ensure it stays to the right in local coordinates
            triangle.transform.localPosition = new Vector3(offsetX, 0, 0);

            // Get the SpriteRenderer of the triangle to apply color changes and fading
            triangleSpriteRenderer = triangle.GetComponent<SpriteRenderer>();
        }
    }
    //change triangle alpha to 0
    public void ChangeTriangleAlphaToZero()
    {
        if (triangleSpriteRenderer != null)
        {
            triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, 0);
        }
    }
    //change triangle alpha to 1
    public void ChangeTriangleAlphaToOne()
    {
        if (triangleSpriteRenderer != null)
        {
            triangleSpriteRenderer.color = new Color(triangleSpriteRenderer.color.r, triangleSpriteRenderer.color.g, triangleSpriteRenderer.color.b, 1);
        }
    }
}
