using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlowButtonHandler : MonoBehaviour
{
    public Image buttonImage;

    public bool DEFAULT = false;
    public bool SLOT2 = false;

    private int currentGlowNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentGlowNumber = PlayerPrefs.GetInt("glowNumber");
        if (DEFAULT && currentGlowNumber == 0)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.8f);
        }
        else if(SLOT2 && currentGlowNumber == 1)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.8f);
        }
        else
        {
            //RBG FORMAT
            buttonImage.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }

    //on button click, load the FAQ scene
    public void OnButtonClick()
    {
        
        Debug.Log("Glow Applied.");
        if(DEFAULT)
        {
            PlayerPrefs.SetInt("glowNumber", 0);
            PlayerPrefs.Save();
        }
        if (SLOT2)
        {
            PlayerPrefs.SetInt("glowNumber", 1);
            PlayerPrefs.Save();
        }
        
    }
}
