using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSlider : MonoBehaviour
{
    public Slider slider; // The UI slider component to show XP progress
    public TextMeshProUGUI levelText; // The UI text component to display the current level

    private int level = 1; // Initial player level
    private float currentXP; // Current accumulated XP of the player
    private float nextLevelXP; // XP required to reach the next level

    void Start()
    {
        // Load XP and level from persistent storage
        LoadPlayerData();

        // Calculate the XP required for the next level based on the loaded level
        nextLevelXP = CalculateNextLevelXP(level);

        // Set the slider's max value to the XP needed for the next level
        slider.maxValue = nextLevelXP;

        // Set the slider's current value to the player's current XP within the level
        slider.value = currentXP % nextLevelXP;

        // Update the displayed level text
        UpdateLevelText();

        Debug.Log("Loaded Data - Current XP: " + currentXP + ", Level: " + level);
    }

    void Update()
    {
        // Continuously update the player's XP from PlayerManager
        if (PlayerManager.instance.currentXP != currentXP)
        {
            UpdateXP(PlayerManager.instance.currentXP);
            SavePlayerData();
        }
        if(level > 1)
        {
            slider.minValue = 100 * (level - 1);
        }       
    }

    // Method to calculate the XP required for the next level
    int CalculateNextLevelXP(int level)
    {
        // XP required for the next level increases by 100 with each level
        return 100 * level;
    }

    // Method to calculate the player's current level based on their total XP
    int CalculateCurrentLevel(int totalXP)
    {
        int level = 1;
        int xpForNextLevel = CalculateNextLevelXP(level);

        while (totalXP >= xpForNextLevel)
        {
            totalXP -= xpForNextLevel;
            level++;
            xpForNextLevel = CalculateNextLevelXP(level);
        }

        return level;
    }

    // Method to update the XP and level dynamically
    void UpdateXP(float xp)
    {
        currentXP = xp;
        bool levelChanged = false;

        // Continuously check if the current XP exceeds the XP required for the next level
        while (currentXP >= nextLevelXP)
        {
            // Subtract the XP needed to reach the next level from currentXP
            currentXP -= nextLevelXP;

            // Increase the player's level
            level++;

            // Calculate the XP required for the next level based on the new level
            nextLevelXP = CalculateNextLevelXP(level);

            // Set a flag indicating that the level has changed
            levelChanged = true;

            Debug.Log("Level Up! New Level: " + level + ", XP Reset to 0 for new level.");
        }

        // Update the slider's maximum value to the XP required for the new level
        slider.maxValue = nextLevelXP;

        // Set the slider's current value to the remaining XP for the current level
        slider.value = currentXP;

        // Update the level text to reflect the new level
        UpdateLevelText();

        // If the level has changed, save the new XP and level data
        if (levelChanged)
        {
            SavePlayerData();
        }
    }

    // Method to handle the visual update of the level text
    void UpdateLevelText()
    {
        levelText.text = "Level " + level;
    }

    // Method to save player data (XP and level) to persistent storage
    void SavePlayerData()
    {
        PlayerPrefs.SetFloat("PlayerXP", currentXP);
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save(); // Ensure data is written to persistent storage
        Debug.Log("Saved Data - Current XP: " + currentXP + ", Level: " + level);
    }

    // Method to load player data (XP and level) from persistent storage
    void LoadPlayerData()
    {
        // Retrieve saved XP and level, or default to 0 XP and level 1 if not set
        currentXP = PlayerPrefs.GetFloat("PlayerXP", 0);
        level = PlayerPrefs.GetInt("PlayerLevel", 1);
    }

    // Save data when the application is quitting
    void OnApplicationQuit()
    {
        Debug.Log("Application Quitting, Saving Data.");
        SavePlayerData();
    }

    // Save data when the application is paused (for example, when it goes to the background on mobile)
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("Application Pausing, Pause Status: " + pauseStatus);
        if (pauseStatus)
        {
            SavePlayerData();
        }
    }
}
