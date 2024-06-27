using System.Collections;
using System.Collections.Generic;
using TMPro;  // TextMeshPro namespace
using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading
using UnityEngine.UI; // For button component

public class MenuButtonsHandler : MonoBehaviour
{
    // Public variables to be set in the inspector
    public TMP_Text buttonText; // Reference to the TMP_Text component
    public string newText; // Text to set on the button
    public TMP_FontAsset newFont; // Font to set on the button
    public string sceneToLoad; // Scene name to load on button click

    void Start()
    {
        // Set the button's text
        if (buttonText != null)
        {
            buttonText.text = newText;
            buttonText.font = newFont;
        }

        // Get the button component and add the onClick listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // Load the specified scene
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log("Loading scene: " + sceneToLoad);
            //SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not specified!");
        }
    }
}
