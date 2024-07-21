using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionHandler : MonoBehaviour
{
    private int levelSelected;
    private Color originalColor;

    public bool level1 = false;
    public bool level2 = false;
    public bool level3 = false;
    public bool level4 = false;
    public bool level5 = false;
    public bool level6 = false;
    public bool level7 = false;
    
    public GameObject Hexagon;
    public GameObject Square;

    private void Start()
    {
        PlayerManager.instance.SetLevelSelected(0);
    }

    private void Update()
    {
        levelSelected = PlayerManager.instance.levelSelected;
        if(level1 && levelSelected == 1)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            
        }
        else if (level2 && levelSelected == 2)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (level3 && levelSelected == 3)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (level4 && levelSelected == 4)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (level5 && levelSelected == 5)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (level6 && levelSelected == 6)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (level7 && levelSelected == 7)
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        }      
        else
        {
            Hexagon.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
            Square.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);           
        }
        
    }
    public void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);       
        if (level1)
        {
            PlayerManager.instance.SetLevelSelected(1);
            AudioManager.Instance.SetBackgroundAudio(AudioManager.Instance.level1Audio);
            AudioManager.Instance.FadeInBackgroundMusic(5f, 0.5f);
        }
        else if (level2)
        {
            PlayerManager.instance.SetLevelSelected(2);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }
        else if (level3)
        {
            PlayerManager.instance.SetLevelSelected(3);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }
        else if (level4)
        {
            PlayerManager.instance.SetLevelSelected(4);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }
        else if (level5)
        {
            PlayerManager.instance.SetLevelSelected(5);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }
        else if (level6)
        {
            PlayerManager.instance.SetLevelSelected(6);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }
        else if (level7)
        {
            PlayerManager.instance.SetLevelSelected(7);
            AudioManager.Instance.FadeOutBackgroundMusic(1f);
        }       
        PlayerManager.instance.SaveScore();
    }
}
