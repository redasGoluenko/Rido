using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class XPCurrent : MonoBehaviour
{
    public Rotate rotate;
    public GoldTokenCurrent goldTokenCurrent;
    public BlueTokenCurrent blueTokenCurrent;
    public PurpleTokenCurrent purpleTokenCurrent;
    public RedTokenCurrent redTokenCurrent;
    public TextMeshProUGUI scoreText;
    public Slider slider;   
    private int currentLevel;
    private float currentXP;
    private bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
        //slider.value = 0;
        StartCoroutine(WaitAndSet(0.5f));       
    }

    IEnumerator WaitAndSet(float delay)
    {           
        yield return new WaitForSeconds(delay);
        currentLevel = rotate.currentLevel;             
    }

    // Update is called once per frame
    void Update()
    {        
        UpdateScoreText();

        // Check if slider value has crossed from >50 to <=50
        if (flag && slider.value <= 50)
        {
            currentLevel++; // Increase currentLevel by 1
            flag = false;   // Set flag to false to prevent further increase until slider goes back >50
        }
        else if (!flag && slider.value > 50)
        {
            flag = true;    // Set flag back to true when slider value goes back >50
        }
    }

    // Method to calculate the total token count
    float GetTotalTokenCount()
    {
        currentXP = PlayerPrefs.GetFloat("PlayerXP", 0);
        while (currentXP >= 100)
        {
            currentXP -= 100;
        }

        float total = currentXP;

        if (goldTokenCurrent != null)
        {
            total += goldTokenCurrent.currentCount * rotate.UnitXPValue;
        }

        if (blueTokenCurrent != null)
        {
            total += (blueTokenCurrent.currentCount * rotate.UnitXPValue) * 5;
        }

        if (purpleTokenCurrent != null)
        {
            total += (purpleTokenCurrent.currentCount * rotate.UnitXPValue) * 10;
        }


        if (redTokenCurrent != null)
        {
            total += (redTokenCurrent.currentCount * rotate.UnitXPValue) * 25;
        }
        return total;
    }

    // Method to update the scoreText with the total token count
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            float totalTokenCount = GetTotalTokenCount();
            while(totalTokenCount >= 100)
            {                                
                totalTokenCount -= 100;                
            }
            slider.value = totalTokenCount;    
            // Update the score text to reflect the new total XP
            scoreText.text = $"Level: {currentLevel}";
        }
    }
}
