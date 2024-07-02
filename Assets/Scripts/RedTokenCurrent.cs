using System.Collections;
using TMPro;
using UnityEngine;

public class RedTokenCurrent : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public PurpleTokenCurrent purpleTokenCurrent;
    public BlueTokenCurrent blueTokenCurrent;
    public GoldTokenCurrent goldTokenCurrent;
    public GameObject redToken;

    public float animationDuration = 1.0f; // Duration of the animation in seconds
    private Coroutine animateCoroutine;
    public bool isDone = false;

    private CanvasRenderer redTokenRenderer; // Reference to the CanvasRenderer component of redToken

    public float pulseSpeed = 2.0f; // Speed of the pulsating effect
    public float pulseMagnitude = 0.1f; // Magnitude of the pulsating effect

    // Start is called before the first frame update
    void Start()
    {
        scoreText.color = new Color32(255, 0, 8, 0); // Start with fully transparent
        scoreText.text = "0"; // Initialize text

        // Get the CanvasRenderer component from the redToken GameObject
        redTokenRenderer = redToken.GetComponent<CanvasRenderer>();
        // Set initial alpha of redToken to 0 (fully transparent)
        redTokenRenderer.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrentRedToken(int targetCount)
    {
        if (targetCount > 0)
        {
            StartCoroutine(UpdateCurrentRedTokenCoroutine(targetCount));
        }
        else
        {
            isDone = true;
        }
    }

    private IEnumerator UpdateCurrentRedTokenCoroutine(int targetCount)
    {
        // Wait until purpleTokenCurrent.isDone becomes true
        while (!purpleTokenCurrent.isDone)
        {
            yield return null;
        }

        // Now execute the animation
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }

        animateCoroutine = StartCoroutine(AnimateScore(targetCount));
    }

    private IEnumerator AnimateScore(int targetCount)
    {
        float timer = 0f;
        float fadeInDuration = 0.125f; // Duration of the fade-in effect (adjust as needed)
        float increment = targetCount / animationDuration;

        // Gradually increase visibility of text and redToken
        while (timer < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            redTokenRenderer.SetAlpha(alpha);

            // Increment timer based on elapsed time
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is fully visible
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        redTokenRenderer.SetAlpha(1f);

        // Number animation with pulsating effect
        int currentCount = 0;
        timer = 0f;
        Vector3 initialScale = scoreText.transform.localScale;

        while (currentCount < targetCount)
        {
            currentCount = Mathf.RoundToInt(timer * increment);

            // Update the score text
            scoreText.text = currentCount.ToString();

            // Pulsating effect: Calculate the scale factor
            float scale = 1 + Mathf.Sin(timer * pulseSpeed) * pulseMagnitude;
            scoreText.transform.localScale = initialScale * scale;

            // Increment timer based on elapsed time
            timer += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final value is set after the loop ends
        scoreText.text = targetCount.ToString();
        scoreText.transform.localScale = initialScale; // Reset to initial scale
        isDone = true;

        StartCoroutine(ContinuePulsating(initialScale));
    }
    private IEnumerator ContinuePulsating(Vector3 initialScale)
    {
        float timer = 0f;
        float halfPulseSpeed = pulseSpeed / 2f; // Halve the pulsation speed

        while (true)
        {
            // Pulsating effect at half speed
            float scale = 1 + Mathf.Sin(timer * halfPulseSpeed) * pulseMagnitude / 3;
            scoreText.transform.localScale = initialScale * scale;

            // Increment timer based on elapsed time
            timer += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
    }
}
