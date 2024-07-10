/* Purpose: handles logic of blue token counter which displays how many blue
tokens the player has collected in his most recent game session */

using System.Collections;
using TMPro;
using UnityEngine;

public class BlueTokenCurrent : MonoBehaviour
{
    public int currentCount = 0; //blue token count starts at 0

    public TextMeshProUGUI scoreText;

    //references to other token counters so blue token counter can wait for them to finish updating before starting its own update
    public PurpleTokenCurrent purpleTokenCurrent; 
    public RedTokenCurrent redTokenCurrent;
    public GoldTokenCurrent goldTokenCurrent;
    public GameObject blueToken;

    public float animationDuration = 1.0f; //how long the counter takes to increase to the target count
    private Coroutine animateCoroutine; //coroutine that animates the counter
    public bool isDone = false; //whether the counter has finished updating

    private CanvasRenderer blueTokenRenderer; //renderer of the blue token image

    public float pulseSpeed = 2.0f; //speed at which the number pulsates after its done increasing
    public float pulseMagnitude = 0.1f; //how much the number pulsates
       
    void Start()
    {
        InitializeBlueTokenCounter();
    }

    //initializes the blue token counter color, text and image alpha
    private void InitializeBlueTokenCounter()
    {
        scoreText.color = new Color32(0, 37, 255, 0);
        scoreText.text = "0";

        blueTokenRenderer = blueToken.GetComponent<CanvasRenderer>();
        blueTokenRenderer.SetAlpha(0f);
    }

    //updates the blue token counter to the target count gradually
    public void UpdateCurrentBlueToken(int targetCount)
    {
        if (targetCount > 0)
        {
            StartCoroutine(UpdateCurrentBlueTokenCoroutine(targetCount));
        }
        else
        {
            isDone = true;
        }
    }

    //coroutine that waits for the other token counters to finish updating before starting its own update
    private IEnumerator UpdateCurrentBlueTokenCoroutine(int targetCount)
    {        
        while (!goldTokenCurrent.isDone)
        {
            yield return null;
        }
      
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }

        animateCoroutine = StartCoroutine(AnimateScore(targetCount));
    }

    //animates the blue token counter to the target count and pulsates the number after its done increasing
    private IEnumerator AnimateScore(int targetCount)
    {
        float timer = 0f;
        float fadeInDuration = 0.125f;
        float increment = targetCount / animationDuration;
       
        while (timer < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);
            blueTokenRenderer.SetAlpha(alpha);
           
            timer += Time.deltaTime;
            yield return null;
        }
       
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        blueTokenRenderer.SetAlpha(1f);
          
        timer = 0f;
        Vector3 initialScale = scoreText.transform.localScale;

        while (currentCount < targetCount)
        {
            currentCount = Mathf.RoundToInt(timer * increment);          
            scoreText.text = currentCount.ToString();          
            float scale = 1 + Mathf.Sin(timer * pulseSpeed) * pulseMagnitude;
            scoreText.transform.localScale = initialScale * scale;          
            timer += Time.deltaTime;
         
            yield return null;
        }
     
        scoreText.text = targetCount.ToString();
        scoreText.transform.localScale = initialScale;
        isDone = true;

        StartCoroutine(ContinuePulsating(initialScale));
    }

    //pulsates the number after its done increasing
    private IEnumerator ContinuePulsating(Vector3 initialScale)
    {
        float timer = 0f;
        float halfPulseSpeed = pulseSpeed / 2f;

        while (true)
        {         
            float scale = 1 + Mathf.Sin(timer * halfPulseSpeed) * pulseMagnitude / 3;
            scoreText.transform.localScale = initialScale * scale;          
            timer += Time.deltaTime;           
            yield return null;
        }
    }
}
