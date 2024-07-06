using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ensure to include this for Image component
using TMPro;

public class ReturnButtonHandler : MonoBehaviour
{
    private float fadeDuration = 1f; // Duration of the fade in seconds

    public TextMeshProUGUI buttonText; // Reference to the store text   
    public Death death; // Reference to the Death script

    private Image backgroundImage; // Reference to the Image component of the background

    void Start()
    {
    }

    void Update()
    {
        // You can put any update logic here if needed
    }

    public void OnButtonClick()
    {       
        // Close the menu
        death.CloseMenu(0);
        // Optionally load the scene after a delay
        StartCoroutine(LoadScene(0.55f)); // Wait 1 second, then load the scene "Menu"
    }

    IEnumerator LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); // Load the scene "Menu"
    }
}
