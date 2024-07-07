using UnityEngine;
using TMPro;
using System.Collections;

public class RetryButtonHandler : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public RectTransform buttonRectTransform; // Reference to the RectTransform component of the button
    public TextMeshProUGUI buttonText; // Reference to the TextMeshProUGUI component of the button

    private bool hasMoved = false; // Flag to track if the button has already moved
    private float moveDistance = 917f; // Distance to move the button
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
            StartCoroutine(WaitAndMoveLeft(0.5f));
            hasMoved = true; // Set the flag to true to indicate movement
        }
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Button clicked");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Endless");
    }

    // Method to move the button to the left
    private void MoveButtonLeft()
    {
        //Debug.Log("Moving button left");
        Vector3 newPosition = buttonRectTransform.anchoredPosition;
        newPosition.x -= moveDistance;
        buttonRectTransform.anchoredPosition = newPosition;
    }

    IEnumerator WaitAndMoveLeft(float delay)
    {
        //Debug.Log("Waiting and moving left");
        yield return new WaitForSeconds(delay);
        MoveButtonLeft();
    }
}
