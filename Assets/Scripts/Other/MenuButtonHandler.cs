using UnityEngine;
using TMPro;
using System.Collections;

public class MenuButtonHandler : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public TextMeshProUGUI buttonText; // Reference to the TextMeshProUGUI component of the button
    public RectTransform buttonRectTransform; // Reference to the RectTransform component of the button

    private bool hasMoved = false; // Flag to track if the button has already moved   
    private float moveDistance = 906f;  // Distance to move the button
    void Start()
    {
        buttonText.text = ""; // Clear the text on the button
        // Get the RectTransform component of the button
        buttonRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Check if rotate object is inactive and the button hasn't moved yet
        if (!rotate.gameObject.activeSelf && !hasMoved)
        {
            //Debug.Log("Rotate object is inactive");
            // Move the button to the left by moveDistance units
            StartCoroutine(WaitAndMoveRight(0.5f));
            hasMoved = true; // Set the flag to true to indicate movement
        }
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        AudioManager.Instance.FadeInBackgroundMusic(1f); // Fade out the background music
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    // Method to move the button to the left
    private void MoveButtonRight()
    {
        Vector3 newPosition = buttonRectTransform.anchoredPosition;
        newPosition.x += moveDistance;
        buttonRectTransform.anchoredPosition = newPosition;
    }

    IEnumerator WaitAndMoveRight(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveButtonRight();
    }
}
