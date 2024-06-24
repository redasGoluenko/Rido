using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyVisiblePivot : MonoBehaviour
{
    public float fadeDuration = 0.0001f;  // Duration of the fade in seconds
    private SpriteRenderer spriteRenderer;
    private Coroutine fadeCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;  // SpriteRenderer needs to be enabled to control its color
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
    }

    void Update()
    {
        // Your regular update logic (if any) goes here
    }

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
