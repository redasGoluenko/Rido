using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PurpleyHandler : MonoBehaviour
{
    private Color purpleColor;
    private Color goldColor;
    private Color blueColor;
    private Color redColor;

    public TrailRenderer purpleTrailRenderer;
    public TrailRenderer goldTrailRenderer;
    public TrailRenderer blueTrailRenderer;
    public TrailRenderer redTrailRenderer;
    public float trailShorteningStep = 0.1f; // Amount to shorten the trail time on each click
    public float minTrailTime = 0.1f; // Minimum allowable trail time to prevent it from disappearing completely
    private bool gold = false;
    private bool blue = false;
    private bool red = false;
    private bool purple = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    public float flashDuration = 0.125f; // Duration of the flash before starting to fade
    public float fadeDuration = 0.5f; // Duration of the fade back to the original color

    void Start()
    {      
        goldColor = goldTrailRenderer.startColor;
        blueColor = blueTrailRenderer.startColor;
        purpleColor = purpleTrailRenderer.startColor;
        redColor = redTrailRenderer.startColor;
        //GET THE SPRITE RENDERER COMPONENT NOT TRAIL RENDERER BUT SPRITE RENDERER
        spriteRenderer = GetComponent<SpriteRenderer>();  
        originalColor = purpleColor;
        SetTrailAlpha(purpleTrailRenderer, 0.75f);

        goldTrailRenderer.emitting = false;
        blueTrailRenderer.emitting = false;
        redTrailRenderer.emitting = false;
    }

    void Update()
    {
        if (purple && Input.touchCount > 0)
        {
            SetTrailAlpha(purpleTrailRenderer, 0f);
        }
        else
        {
            SetTrailAlpha(purpleTrailRenderer, 0.75f);
        }
        // Detect touch on mobile or click on desktop
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (gold)
            {
                StartCoroutine(ToggleEmissionFor(goldTrailRenderer, 0.25f));
                if(flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                flashCoroutine = StartCoroutine(FlashToColor(goldColor, flashDuration, fadeDuration));
            }
            if (blue)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                flashCoroutine = StartCoroutine(FlashToColor(blueColor, flashDuration, fadeDuration));
                StartCoroutine(ToggleEmissionFor(blueTrailRenderer, 0.25f));
            }
            if (red)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                flashCoroutine = StartCoroutine(FlashToColor(redColor, flashDuration, fadeDuration));
                StartCoroutine(ToggleEmissionFor(redTrailRenderer, 0.25f));
            }                    
            AdjustTrailTime(purpleTrailRenderer);
            AdjustTrailTime(goldTrailRenderer);          
            AdjustTrailTime(blueTrailRenderer);
            AdjustTrailTime(redTrailRenderer);
        }
    }

    IEnumerator ToggleEmissionFor(TrailRenderer trailRenderer, float duration)
    {
        trailRenderer.emitting = true;       
        yield return new WaitForSeconds(duration);
        trailRenderer.emitting = false;
    }

    // Adjust the trail time to shorten it
    void AdjustTrailTime(TrailRenderer trailRenderer)
    {
        // Reduce the trail time by the defined step
        float newTrailTime = Mathf.Max(trailRenderer.time - trailShorteningStep, minTrailTime);
        trailRenderer.time = newTrailTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            gold = true;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            blue = true;
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            red = true;
        }
        if(collision.gameObject.CompareTag("HoldToken"))
        {
            purple = true;
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            gold = false;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            blue = false;
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            red = false;
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            purple = false;
        }
    }

    // Set the alpha for a TrailRenderer
    public void SetTrailAlpha(TrailRenderer trailRenderer, float alpha)
    {
        Color startColor = trailRenderer.startColor;
        startColor.a = alpha;
        trailRenderer.startColor = startColor;

        Color endColor = trailRenderer.endColor;
        endColor.a = alpha;
        trailRenderer.endColor = endColor;

        // Alternatively, if you want to control the material color directly
        if (trailRenderer.material.HasProperty("_Color"))
        {
            Color color = trailRenderer.material.color;
            color.a = alpha;
            trailRenderer.material.color = color;
        }
    }
    IEnumerator FlashToColor(Color targetColor, float flashDuration, float fadeDuration)
    {
        // Set the color to the target color
        spriteRenderer.color = targetColor;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);
        //lerp to the original color
        float timeElapsed = 0;
        while (timeElapsed < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }      
    }


}
