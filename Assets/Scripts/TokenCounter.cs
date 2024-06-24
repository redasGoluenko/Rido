using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TokenCounter : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public TextMeshProUGUI textMeshPro; // Reference to the TextMeshProUGUI component  
    public RectTransform tokenCounterRectTransform; // Reference to the RectTransform of the token counter

    private bool hasMoved = false; // Flag to track if the token counter has already moved  

    void Start()
    {
        // Get the RectTransform component of the token counter
        tokenCounterRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotate.gameObject.activeSelf && !hasMoved)
        {
            Debug.Log("Rotate object is inactive");
            // Move the button to the left by moveDistance units
            MoveCounter();
            hasMoved = true; // Set the flag to true to indicate movement
        }

        // Update the text to display the current token count
        textMeshPro.text = rotate.tokenCount.ToString();
    }
    
    // Method to move the token counter
    private void MoveCounter()
    {
        Vector3 newPosition = tokenCounterRectTransform.anchoredPosition;
        
        if(rotate.tokenCount > 99)
        {
            newPosition.x += 90f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 300;
        }
        else if(rotate.tokenCount > 9)
        {
            newPosition.x += 130f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 500;
        }
        else
        {
            newPosition.x += 185f;
            newPosition.y -= 200f;
            textMeshPro.fontSize = 700;
        }
        
        tokenCounterRectTransform.anchoredPosition = newPosition;
    }

    // Method to change the color of the text
    public void ColorBlue()
    {
        textMeshPro.color = Color.blue;
    }
    // Method to change the color of the text
    public void ColorYellow()
    {
        textMeshPro.color = Color.yellow;
    }
    // Method to change the color of the text
    public void ColorWhite()
    {
        textMeshPro.color = Color.white;
    }
    // Method to change the color of the text
    public void ColorPurple()
    {
       textMeshPro.color = new Color(0.5f, 0, 0.5f);
    }
}
