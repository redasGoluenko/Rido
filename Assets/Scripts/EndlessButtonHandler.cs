using UnityEngine;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {       
        UnityEngine.SceneManagement.SceneManager.LoadScene("Endless");
    }
}
