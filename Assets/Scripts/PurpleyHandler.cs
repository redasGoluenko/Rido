using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PurpleyHandler : MonoBehaviour
{
    private Color purpleColor;
    private Color goldColor;
    private Color blueColor;
    private Color redColor;

    public SpriteRenderer OuterLeft;
    public SpriteRenderer OuterRight;
    public SpriteRenderer InnerLeft;
    public SpriteRenderer InnerRight;
    public TrailRenderer purpleTrailRenderer;
    public TrailRenderer goldTrailRenderer;
    public TrailRenderer blueTrailRenderer;
    public TrailRenderer redTrailRenderer;
    public TrailRenderer darkPurpleTrailRenderer;
    public float trailShorteningStep = 0.1f; // Amount to shorten the trail time on each click
    public float minTrailTime = 0.1f; // Minimum allowable trail time to prevent it from disappearing completely
    private bool gold = false;
    private bool blue = false;
    private bool red = false;
    private bool purple = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private TrailRenderer currentTrailRenderer;
    private Color currentColor;

    public float flashDuration = 0.125f; // Duration of the flash before starting to fade
    public float fadeDuration = 0.5f; // Duration of the fade back to the original color

    private bool isGlowSelection = false;
    private bool isEndless = false;
    private int glowNumber;

    void Start()
    {           

        if (SceneManager.GetActiveScene().name == "Glows")
        {
            isGlowSelection = true;
        }
        else
        {
            glowNumber = PlayerManager.instance.glowNumber;
        }
        if(SceneManager.GetActiveScene().name == "Endless")
        {
            isEndless = true;
        }
        goldColor = goldTrailRenderer.startColor;
        blueColor = blueTrailRenderer.startColor;
        purpleColor = purpleTrailRenderer.startColor;
        redColor = redTrailRenderer.startColor;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = purpleColor;
        SetTrailAlpha(purpleTrailRenderer, 0.75f);

        // Initially, only the dark purple trail should be emitting
        goldTrailRenderer.emitting = false;
        blueTrailRenderer.emitting = false;
        redTrailRenderer.emitting = false;
        darkPurpleTrailRenderer.emitting = false;

        currentTrailRenderer = goldTrailRenderer;
        currentColor = goldColor;
    }

    void Update()
    {
        if (isGlowSelection)
        {
            if (PlayerPrefs.GetInt("glowNumber") == 4)
            {
                HandleColorFlashAndEmission();

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    StartCoroutine(ToggleEmissionFor(currentTrailRenderer, 0.25f));
                    if (flashCoroutine != null)
                    {
                        StopCoroutine(flashCoroutine);
                    }
                    flashCoroutine = StartCoroutine(FlashToColor(currentColor, flashDuration, fadeDuration));          
                }
            }
        }
        else
        {
            HandleColorFlashAndEmission();

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartCoroutine(ToggleEmissionFor(currentTrailRenderer, 0.25f));
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                flashCoroutine = StartCoroutine(FlashToColor(currentColor, flashDuration, fadeDuration));
                
                if (isEndless)
                {
                    AdjustTrailTime(purpleTrailRenderer);
                    AdjustTrailTime(goldTrailRenderer);
                    AdjustTrailTime(blueTrailRenderer);
                    AdjustTrailTime(redTrailRenderer);
                    AdjustTrailTime(darkPurpleTrailRenderer);
                }
            }
        }
    }

    void HandleColorFlashAndEmission()
    {
        if (gold)
        {
            currentTrailRenderer = goldTrailRenderer;
            currentColor = goldColor;
        }
        if (blue)
        {
            currentTrailRenderer = blueTrailRenderer;
            currentColor = blueColor;
        }
        if (red)
        {
            currentTrailRenderer = redTrailRenderer;
            currentColor = redColor;
        }
        if (purple)
        {
            darkPurpleTrailRenderer.emitting = true;
        }
        else
        {
            darkPurpleTrailRenderer.emitting = false;
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
        // Only adjust if the trail renderer is currently emitting
        if (trailRenderer.emitting)
        {
            float newTrailTime = Mathf.Max(trailRenderer.time - trailShorteningStep, minTrailTime);
            trailRenderer.time = newTrailTime;
        }
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
        if (collision.gameObject.CompareTag("HoldToken"))
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
        InnerLeft.color = targetColor;
        InnerRight.color = targetColor;
        OuterLeft.color = targetColor;
        OuterRight.color = targetColor;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Lerp to the original color
        float timeElapsed = 0;
        while (timeElapsed < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, timeElapsed / fadeDuration);
            InnerLeft.color = Color.Lerp(targetColor, Color.white, timeElapsed / fadeDuration);
            InnerRight.color = Color.Lerp(targetColor, Color.white, timeElapsed / fadeDuration);
            OuterLeft.color = Color.Lerp(targetColor, Color.white, timeElapsed / fadeDuration);
            OuterRight.color = Color.Lerp(targetColor, Color.white, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the color is set back to the original color
        spriteRenderer.color = originalColor;
        InnerLeft.color = Color.white;
        InnerRight.color = Color.white;
        OuterLeft.color = Color.white;
        OuterRight.color = Color.white;
    }
}
