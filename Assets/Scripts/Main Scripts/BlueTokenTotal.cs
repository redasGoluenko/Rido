/* Purpose: handles logic of blue token counter which displays how many blue tokens the player has collected in total */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueTokenTotal : MonoBehaviour
{   
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.color = Color.blue;
        UpdateScore();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (PlayerManager.instance != null && scoreText != null)
        {
            scoreText.text = "" + PlayerManager.instance.currentBlueTokens;
        }
    }
}
