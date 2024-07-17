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
    public bool retried = false;
    float previousSliderValue;

    void Start()
    {
        currentLevel = PlayerManager.instance.LevelInstance;
        if(slider.value <= 50)
        {
            flag = false;
        }
        previousSliderValue = slider.value;
    } 
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

        if (slider.value != previousSliderValue)
        {
            AudioManager.Instance.SFX.volume = 0.2f;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.tokenPickup);
            previousSliderValue = slider.value; // Update previous slider value
        }
    }

    // Method to calculate the total token count
    float GetTotalTokenCount()
    {
        currentXP = PlayerManager.instance.XPInstance;      
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
            if(retried)
            {
                retried = false;
                PlayerManager.instance.SetXP(totalTokenCount);
                PlayerManager.instance.SetLevel(currentLevel);                
            }
            while(totalTokenCount >= 100)
            {                                
                totalTokenCount -= 100;                
            }
            slider.value = totalTokenCount;           
            scoreText.text = $"Level: {currentLevel}";
        }
    }
}
