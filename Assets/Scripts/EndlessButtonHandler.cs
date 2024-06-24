using UnityEngine;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void OnButtonClick()
    {       
        UnityEngine.SceneManagement.SceneManager.LoadScene("Endless"); // Load the Endless scene
    }
}
