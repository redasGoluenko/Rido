using System.Collections;
using UnityEngine;

public class GoldTokenCurrent : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    public PurpleTokenCurrent purpleTokenCurrent;
    public RedTokenCurrent redTokenCurrent;
    public BlueTokenCurrent blueTokenCurrent;
    public GameObject goldToken;

    public float animationDuration = 1.0f; // Duration of the animation in seconds
    private Coroutine animateCoroutine;
    public bool isDone = false;

    private CanvasRenderer goldTokenRenderer; // Reference to the CanvasRenderer component of goldToken

    // Start is called before the first frame update
    void Start()
    {
        // Initialize text
        scoreText.text = "0";
        // Set initial color (yellow) and make it fully transparent
        scoreText.color = new Color32(255, 203, 0, 0); // Alpha set to 0 (fully transparent)

        // Get the CanvasRenderer component from the goldToken GameObject
        goldTokenRenderer = goldToken.GetComponent<CanvasRenderer>();
        // Set initial alpha of goldToken to 0 (fully transparent)
        goldTokenRenderer.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateCurrentGoldToken(int targetCount)
    {
        if(targetCount > 0)
        {
            StartCoroutine(AnimateScore(targetCount));
        }    
        else
        {
            isDone = true;
        }
    }

    private IEnumerator AnimateScore(int targetCount)
    {
        float timer = 0f;
        float fadeInDuration = 0.5f; // Duration of the fade-in effect (adjust as needed)
        float increment = targetCount / animationDuration;

        // Gradually increase visibility of text and goldToken
        while (timer < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            goldTokenRenderer.SetAlpha(alpha);

            // Increment timer based on elapsed time
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is fully visible
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        goldTokenRenderer.SetAlpha(1f);

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
