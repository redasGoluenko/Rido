using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int currentGoldTokens = 0;
    public int currentBlueTokens = 0;
    public int currentPurpleTokens = 0;
    public int currentRedTokens = 0;
    public float currentXP = 0;
    public int glowNumber = 0;
    public int playerLevel = 1;
    public int levelSelected = 0;

    public float XPInstance = 0;
    public int LevelInstance = 1;

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
        glowNumber = 0;
        playerLevel = 1;
        levelSelected = 0;

        XPInstance = 0;
        LevelInstance = 1;
        PlayerPrefs.DeleteAll();  // Remove all saved data
        PlayerPrefs.Save();  // Ensure the changes are written to disk

        Debug.Log("Score has been reset.");

        // Optionally, find and update the ScoreTotal in the current scene
        GoldTokenTotal goldTokenTotal = Object.FindAnyObjectByType<GoldTokenTotal>();
        BlueTokenTotal blueTokenTotal = Object.FindAnyObjectByType<BlueTokenTotal>();
        PurpleTokenTotal purpleTokenTotal = Object.FindAnyObjectByType<PurpleTokenTotal>();
        RedTokenTotal redTokenTotal = Object.FindAnyObjectByType<RedTokenTotal>();
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
    public void SetGlowNumber(int number)
    {
        glowNumber = number;
        SaveScore();
    }
    public void SetLevel(int level)
    {
        LevelInstance = level;
        SaveScore();
    }
    public void SetXP(float xp)
    {
        XPInstance = xp;
        SaveScore();
    }
    public void SetLevelSelected(int level)
    {
        levelSelected = level;
        SaveScore();
    }


    public void SaveScore()
    {
        PlayerPrefs.SetInt("GoldTokens", currentGoldTokens);
        PlayerPrefs.SetInt("BlueTokens", currentBlueTokens);
        PlayerPrefs.SetInt("PurpleTokens", currentPurpleTokens);
        PlayerPrefs.SetInt("RedTokens", currentRedTokens);
        PlayerPrefs.SetInt("glowNumber", glowNumber);
        PlayerPrefs.SetFloat("XP", currentXP); 
        PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        PlayerPrefs.SetFloat("PlayerXP",  XPInstance);
        PlayerPrefs.SetInt("Level", LevelInstance);
        PlayerPrefs.SetInt("LevelSelected", levelSelected);
        PlayerPrefs.Save();
    }

    public void LoadScore()
    {
        if (PlayerPrefs.HasKey("GoldTokens"))
        {
            currentGoldTokens = PlayerPrefs.GetInt("GoldTokens");
            Debug.Log("Gold Tokens: " + currentGoldTokens);
        }
        if (PlayerPrefs.HasKey("BlueTokens"))
        {
            currentBlueTokens = PlayerPrefs.GetInt("BlueTokens");
            Debug.Log("Blue Tokens: " + currentBlueTokens);
        }
        if (PlayerPrefs.HasKey("PurpleTokens"))
        {
            currentPurpleTokens = PlayerPrefs.GetInt("PurpleTokens");
            Debug.Log("Purple Tokens: " + currentPurpleTokens);
        }
        if (PlayerPrefs.HasKey("RedTokens"))
        {
            currentRedTokens = PlayerPrefs.GetInt("RedTokens");
            Debug.Log("Red Tokens: " + currentRedTokens);
        }
        if (PlayerPrefs.HasKey("XP"))
        {
            currentXP = PlayerPrefs.GetFloat("XP");
            Debug.Log("XP: " + currentXP);
        }  
        if (PlayerPrefs.HasKey("glowNumber"))
        {
            glowNumber = PlayerPrefs.GetInt("glowNumber");
            Debug.Log("Glow Number: " + glowNumber);
        }
        if (PlayerPrefs.HasKey("PlayerLevel"))
        {
            playerLevel = PlayerPrefs.GetInt("PlayerLevel");
            Debug.Log("Player Level: " + playerLevel);
        }  
        if(PlayerPrefs.HasKey("PlayerXP"))
        {
            XPInstance = PlayerPrefs.GetFloat("PlayerXP");
        }
        if (PlayerPrefs.HasKey("Level"))
        {
            LevelInstance = PlayerPrefs.GetInt("Level");
        }
        if (PlayerPrefs.HasKey("LevelSelected"))
        {
            levelSelected = PlayerPrefs.GetInt("LevelSelected");
        }
    }

    private void OnApplicationQuit()
    {
        SaveScore();
    }
}
