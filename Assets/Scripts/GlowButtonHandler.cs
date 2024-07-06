using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlowButtonHandler : MonoBehaviour
{
    public Image buttonImage;

    public bool SLOT1 = false;
    public bool SLOT2 = false;
    public bool SLOT3 = false;

    private int currentGlowNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentGlowNumber = PlayerPrefs.GetInt("glowNumber");
        if (SLOT1 && currentGlowNumber == 0)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if(SLOT2 && currentGlowNumber == 1)
        {
            buttonImage.color = new Color(0f, 0f, 0f, 0.1f);
        }
        else if (SLOT3 && currentGlowNumber == 2)
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
        if(SLOT1)
        {
            PlayerPrefs.SetInt("glowNumber", 0);
            PlayerPrefs.Save();
        }
        if (SLOT2)
        {
            PlayerPrefs.SetInt("glowNumber", 1);
            PlayerPrefs.Save();
        }
        if (SLOT3)
        {
            PlayerPrefs.SetInt("glowNumber", 2);
            PlayerPrefs.Save();
        }
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {       
        Debug.Log("Collision Detected");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with " + other.gameObject.name);
    }
}
