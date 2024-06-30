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
        // Initialize current XP from the PlayerManager
        currentXP = PlayerManager.instance.currentXP;

        // Calculate the XP required to reach the next level for the initial level
        nextLevelXP = CalculateNextLevelXP(level);

        // Set the slider's max value to the XP needed for the next level
        slider.maxValue = nextLevelXP;

        // Set the slider's current value to the player's current XP
        slider.value = currentXP;

        // Update the displayed level text
        UpdateLevelText();

        Debug.Log("Current XP: " + currentXP);
    }

    void Update()
    {
        // Continuously update the player's XP from PlayerManager
        // This assumes PlayerManager.instance.currentXP is kept up-to-date
        UpdateXP(PlayerManager.instance.currentXP);
    }

    // Method to calculate the XP required for the next level
    int CalculateNextLevelXP(int level)
    {
        // XP required for the next level increases by 100 with each level
        return 100 * level;
    }

    // Method to update the XP and level dynamically
    void UpdateXP(int xp)
    {
        // Update the current XP with the new value from the player manager
        currentXP = xp;

        // Handle leveling up: if current XP exceeds or meets the XP needed for the next level
        while (currentXP >= nextLevelXP)
        {
            // Subtract the XP required for the current level and proceed to the next level
            currentXP -= nextLevelXP;

            // Update the PlayerManager's XP to the remaining current XP
            PlayerManager.instance.currentXP = currentXP;

            // Increment the level
            level++;

            // Recalculate the XP required for the next level
            nextLevelXP = CalculateNextLevelXP(level);

            // Log level up event
            Debug.Log("Leveled up to Level " + level + "! Current XP carried over: " + currentXP);
        }

        // Update the PlayerManager's XP to the latest value
        PlayerManager.instance.currentXP = currentXP;

        // Update the slider to reflect the current XP relative to the current level's requirement
        slider.maxValue = nextLevelXP;
        slider.value = currentXP;

        // Update the level text to reflect the new level
        UpdateLevelText();
    }

    // Method to handle the visual update of the level text
    void UpdateLevelText()
    {
        // Display the current level in the UI text component
        levelText.text = "Level " + level;
    }
}
