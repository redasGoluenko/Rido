using System.Collections;
using TMPro;
using UnityEngine;

public class BlueTokenCurrent : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public PurpleTokenCurrent purpleTokenCurrent;
    public RedTokenCurrent redTokenCurrent;
    public GoldTokenCurrent goldTokenCurrent;
    public GameObject blueToken;
    public XPEarned XPEarned;

    public float animationDuration = 1.0f; // Duration of the animation in seconds
    private Coroutine animateCoroutine;
    public bool isDone = false;

    private CanvasRenderer blueTokenRenderer; // Reference to the CanvasRenderer component of blueToken

    // Start is called before the first frame update
    void Start()
    {
        scoreText.color = new Color32(0, 37, 255, 0); // Start with fully transparent
        scoreText.text = "0"; // Initialize text

        // Get the CanvasRenderer component from the blueToken GameObject
        blueTokenRenderer = blueToken.GetComponent<CanvasRenderer>();
        // Set initial alpha of blueToken to 0 (fully transparent)
        blueTokenRenderer.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrentBlueToken(int targetCount)
    {
        if(targetCount > 0)
        {
            StartCoroutine(UpdateCurrentBlueTokenCoroutine(targetCount));
        }     
    }

    private IEnumerator UpdateCurrentBlueTokenCoroutine(int targetCount)
    {
        // Wait until goldTokenCurrent.isDone becomes true
        while (!goldTokenCurrent.isDone)
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
        float fadeInDuration = 1.0f; // Duration of the fade-in effect (adjust as needed)
        float increment = targetCount / animationDuration;

        // Gradually increase visibility of text and blueToken
        while (timer < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            blueTokenRenderer.SetAlpha(alpha);

            // Increment timer based on elapsed time
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is fully visible
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        blueTokenRenderer.SetAlpha(1f);

        // Number animation
        int currentCount = 0;
        timer = 0f;
        while (currentCount < targetCount)
        {
            currentCount = Mathf.RoundToInt(timer * increment);

            // Update the score text
            scoreText.text = currentCount.ToString();

            // Increment timer based on elapsed time
            timer += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final value is set after the loop ends
        scoreText.text = targetCount.ToString();
        isDone = true;
    }
}
