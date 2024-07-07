using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private bool isColliding = false;
    private bool isCollidingHoldToken = false;
    private Coroutine fadeCoroutine;

    void Update()
    {
        if (isColliding && Input.touchCount > 0)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            // Start the flash effect: set alpha to 1 and begin fading out
            SetAlpha(1f);
            fadeCoroutine = StartCoroutine(FadeOut(0.5f)); // Adjust the duration as needed

            // Since the object will be destroyed on touch, we reset the collision state
            isColliding = false;
        }
        else if (!isColliding)
        {
            // Ensure alpha is set to 0 when not colliding or flash is not active
            SetAlpha(0f);
        }

        while(isCollidingHoldToken && Input.touchCount > 0)
        {            
            SetAlpha(1f);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            isColliding = true;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            isColliding = true;
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            isColliding = true;
            isCollidingHoldToken = true;
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            isColliding = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            isColliding = false;
            isCollidingHoldToken = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }        
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = 1f; // Start from fully visible since we set it to 1f on touch

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            SetAlpha(newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is set to 0 at the end of the fade
        SetAlpha(0f);
        fadeCoroutine = null; // Reset the coroutine reference
    }

    private void SetAlpha(float alpha)
    {
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) // Ensure there's a SpriteRenderer component
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }
    }
}
