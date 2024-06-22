using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TokenCounter : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public TextMeshProUGUI textMeshPro; // Reference to the TextMeshProUGUI component  

    // Update is called once per frame
    void Update()
    {
        // Update the text to display the current token count
        textMeshPro.text = rotate.tokenCount.ToString();
    }

    public void ColorBlue()
    {
        textMeshPro.color = Color.blue;
    }
    public void ColorYellow()
    {
        textMeshPro.color = Color.yellow;
    }
    public void ColorWhite()
    {
        textMeshPro.color = Color.white;
    }
}
