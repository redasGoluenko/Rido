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
    private int currentXP; // Current accumulated XP of the player
    private int nextLevelXP; // XP required to reach the next level

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
        UpdateXP(PlayerManager.instance.currentXP);
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
    void UpdateXP(int xp)
    {
        currentXP = xp;

        // Handle leveling up: if current XP exceeds or meets the XP needed for the next level
        while (currentXP >= nextLevelXP)
        {
            currentXP -= nextLevelXP;

            // Update the PlayerManager's XP to the remaining current XP
            PlayerManager.instance.currentXP = currentXP;

            level++;
            nextLevelXP = CalculateNextLevelXP(level);
        }

        // Update the PlayerManager's XP to the latest value
        PlayerManager.instance.currentXP = currentXP;

        // Update the slider to reflect the XP within the current level range
        slider.maxValue = nextLevelXP;
        slider.value = currentXP % nextLevelXP;

        // Update the level text to reflect the new level
        UpdateLevelText();
    }

    // Method to handle the visual update of the level text
    void UpdateLevelText()
    {
        levelText.text = "Level " + level;
    }

    // Method to save player data (XP and level) to persistent storage
    void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save(); // Ensure data is written to persistent storage
        Debug.Log("Saved Data - Current XP: " + currentXP + ", Level: " + level);
    }

    // Method to load player data (XP and level) from persistent storage
    void LoadPlayerData()
    {
        // Retrieve saved XP and level, or default to 0 XP and level 1 if not set
        currentXP = PlayerPrefs.GetInt("PlayerXP", 0);
        level = PlayerPrefs.GetInt("PlayerLevel", 1);
    }

    // Save data when the application is quitting
    void OnApplicationQuit()
    {
        SavePlayerData();
    }

    // Save data when the application is paused (for example, when it goes to the background on mobile)
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SavePlayerData();
        }
    }
}
