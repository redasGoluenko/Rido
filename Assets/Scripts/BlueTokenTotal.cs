using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueTokenTotal : MonoBehaviour
{
    // Score Text component reference
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
