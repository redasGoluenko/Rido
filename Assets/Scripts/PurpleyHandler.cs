using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleyHandler : MonoBehaviour
{
    public TrailRenderer purpleTrailRenderer;
    public TrailRenderer goldTrailRenderer;
    public TrailRenderer blueTrailRenderer;
    public TrailRenderer redTrailRenderer;
    public float trailShorteningStep = 0.1f; // Amount to shorten the trail time on each click
    public float minTrailTime = 0.1f; // Minimum allowable trail time to prevent it from disappearing completely
    private bool gold = false;
    private bool blue = false;
    private bool red = false;

    void Start()
    {       
        SetTrailAlpha(purpleTrailRenderer, 0.75f);

        goldTrailRenderer.emitting = false;
        blueTrailRenderer.emitting = false;
        redTrailRenderer.emitting = false;
    }

    void Update()
    {
        // Detect touch on mobile or click on desktop
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (gold)
            {
                StartCoroutine(ToggleEmissionFor(goldTrailRenderer, 0.25f));
            }
            if (blue)
            {
                StartCoroutine(ToggleEmissionFor(blueTrailRenderer, 0.25f));
            }
            if (red)
            {
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
}
