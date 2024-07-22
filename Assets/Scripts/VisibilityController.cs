using UnityEngine;
using System.Collections;

public class VisibilityController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeInDuration = 1f; // Duration of the fade effect
    public float fadeOutDuration = 1f;  

    private Coroutine fadeCoroutine; // To keep track of the active coroutine

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize the visibility based on the levelSelected value
        if (PlayerManager.instance.levelSelected == 1)
        {
            SetAlpha(1f);
        }
        else
        {
            SetAlpha(0f);
        }
    }

    void Update()
    {
        // Check the levelSelected value every frame
        if (PlayerManager.instance.levelSelected == 1)
        {
            if (spriteRenderer.color.a < 1f)
            {              
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }

                fadeCoroutine = StartCoroutine(FadeTo(1f, fadeInDuration));
            }
        }
        else
        {
            if (spriteRenderer.color.a > 0f)
            {
                // Stop any existing fade coroutine if it's running
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
               
                SetAlpha(0f);
            }
        }
    }

    private IEnumerator FadeTo(float targetAlpha, float fadeDuration)
    {            
        float startAlpha = spriteRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha); // Ensure the final alpha is set
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
