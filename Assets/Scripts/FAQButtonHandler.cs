using UnityEngine;
using TMPro;
using System.Collections;
using UnityEditor;

public class FAQButtonHandler : MonoBehaviour
{
    int number = 0;
    // Start is called before the first frame update
    void Start()
    {
        //playerprefs get int
        int glowNumber = PlayerPrefs.GetInt("glowNumber", 0);
        number = glowNumber;
        Debug.Log("Start Glow Number: " + glowNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //on button click, load the FAQ scene
    public void OnButtonClick()
    {      
        if(number == 1)
        {
            number = 0;
        }
        else
        {
            number = 1;
        }
        PlayerPrefs.SetInt("glowNumber", number);
        Debug.Log("Glow Number: " + number);
    }
}
