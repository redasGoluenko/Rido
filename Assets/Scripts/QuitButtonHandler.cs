using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuitButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        Application.Quit();
    }
}
