using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ensure to include this for Image component
using TMPro;
using UnityEngine.SceneManagement;

public class ReturnButtonHandler : MonoBehaviour
{
    public bool retract = false;
    public TextMeshProUGUI buttonText; // Reference to the store text   
    public Death death; // Reference to the Death script   

    private Image backgroundImage; // Reference to the Image component of the background
    private bool inGlowSelection = false;

    void Start()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.whoosh);
        if (SceneManager.GetActiveScene().name == "Glows")
        {
            inGlowSelection = true;
        }      
    }

    public void OnButtonClick()
    {           
        retract = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        AudioManager.Instance.SetBackgroundAudio(AudioManager.Instance.mainMenu);
        PlayerManager.instance.SetLevelSelected(0);
        if (!inGlowSelection) { AudioManager.Instance.FadeInBackgroundMusic(1f, 1f); }
        death.CloseMenu(0);       
        StartCoroutine(LoadScene(0.55f));
    }

    IEnumerator LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); // Load the scene "Menu"
    }
}
