using JetBrains.Annotations;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int currentGoldTokens = 0;
    public int currentBlueTokens = 0;
    public int currentPurpleTokens = 0;
    public int currentRedTokens = 0;
    public float currentXP = 0;

    public bool reset = false;

    private void Start()
    {
        if (reset)
        {
            ResetScore();
        }       
    }
    public void ResetScore()
    {
        currentGoldTokens = 0;
        currentBlueTokens = 0;
        currentPurpleTokens = 0;
        currentRedTokens = 0;
        currentXP = 0;
        PlayerPrefs.DeleteAll();  // Remove all saved data
        PlayerPrefs.Save();  // Ensure the changes are written to disk

        Debug.Log("Score has been reset.");

        // Optionally, find and update the ScoreTotal in the current scene
        GoldTokenTotal goldTokenTotal = FindObjectOfType<GoldTokenTotal>();
        BlueTokenTotal blueTokenTotal = FindObjectOfType<BlueTokenTotal>();
        PurpleTokenTotal purpleTokenTotal = FindObjectOfType<PurpleTokenTotal>();
        RedTokenTotal redTokenTotal = FindObjectOfType<RedTokenTotal>();
        XPTotal xpTotal = FindAnyObjectByType<XPTotal>();
        if (goldTokenTotal != null)
        {
            goldTokenTotal.UpdateScore();
            blueTokenTotal.UpdateScore();
            purpleTokenTotal.UpdateScore();
            redTokenTotal.UpdateScore();
            xpTotal.UpdateScore();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadScore();
    }

    public void AddGoldTokens(int points)
    {       
        currentGoldTokens += points;
        SaveScore();
    }
    public void AddBlueTokens(int points)
    {
        currentBlueTokens += points;
        SaveScore();
    }
    public void AddPurpleTokens(int points)
    {
        currentPurpleTokens += points;
        SaveScore();
    }
    public void AddRedTokens(int points)
    {
        currentRedTokens += points;
        SaveScore();
    }

    public void AddXP(float points)
    {
        currentXP += points;
        SaveScore();
    }


    public void SaveScore()
    {
        PlayerPrefs.SetInt("GoldTokens", currentGoldTokens);
        PlayerPrefs.SetInt("BlueTokens", currentBlueTokens);
        PlayerPrefs.SetInt("PurpleTokens", currentPurpleTokens);
        PlayerPrefs.SetInt("RedTokens", currentRedTokens);
        PlayerPrefs.SetFloat("XP", currentXP);
        PlayerPrefs.Save();
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("GoldTokens"))
        {
            currentGoldTokens = PlayerPrefs.GetInt("GoldTokens");
        }
        if (PlayerPrefs.HasKey("BlueTokens"))
        {
            currentBlueTokens = PlayerPrefs.GetInt("BlueTokens");
        }
        if (PlayerPrefs.HasKey("PurpleTokens"))
        {
            currentPurpleTokens = PlayerPrefs.GetInt("PurpleTokens");
        }
        if (PlayerPrefs.HasKey("RedTokens"))
        {
            currentRedTokens = PlayerPrefs.GetInt("RedTokens");
        }
        if (PlayerPrefs.HasKey("XP"))
        {
            currentXP = PlayerPrefs.GetFloat("XP");
        }
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }
}
