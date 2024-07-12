using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlowButtonHandler : MonoBehaviour
{
    public Image buttonImage;

    public bool SLOT0 = false;
    public bool SLOT1 = false;
    public bool SLOT2 = false;
    public bool SLOT3 = false;
    public bool SLOT4 = false;
    public bool SLOT5 = false;

    private int currentGlowNumber;  

    void Update()
    {
        currentGlowNumber = PlayerManager.instance.glowNumber;
        if (SLOT0 && currentGlowNumber == 0)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if(SLOT1 && currentGlowNumber == 1)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if (SLOT2 && currentGlowNumber == 2)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if (SLOT3 && currentGlowNumber == 3)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if (SLOT4 && currentGlowNumber == 4)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if (SLOT5 && currentGlowNumber == 5)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.8f);           
        }
    }

    //on button click, load the FAQ scene
    public void OnButtonClick()
    {      
        Debug.Log("Glow Applied.");
        if(SLOT0)
        {
            PlayerManager.instance.SetGlowNumber(0);          
        }
        else if (SLOT1)
        {
            PlayerManager.instance.SetGlowNumber(1);
        }
        else if (SLOT2)
        {
            PlayerManager.instance.SetGlowNumber(2);
        }
        else if (SLOT3)
        {
            PlayerManager.instance.SetGlowNumber(3);
        }
        else if (SLOT4)
        {
            PlayerManager.instance.SetGlowNumber(4);
        }
        else if (SLOT5)
        {
            PlayerManager.instance.SetGlowNumber(5);
        }
        PlayerManager.instance.SaveScore();
    }
}
