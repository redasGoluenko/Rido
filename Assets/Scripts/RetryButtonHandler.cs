using UnityEngine;
using TMPro;

public class RetryButtonHandler : MonoBehaviour
{
    public Rotate rotate;
    public TextMeshProUGUI buttonText;

    private bool hasMoved = false; // Flag to track if the button has already moved

    // Distance to move the button if rotate is inactive
    private float moveDistance = 917f;

    // Reference to the RectTransform of the button
    public RectTransform buttonRectTransform;

    void Start()
    {
        // Get the RectTransform component of the button
        buttonRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Check if rotate object is inactive and the button hasn't moved yet
        if (!rotate.gameObject.activeSelf && !hasMoved)
        {
            Debug.Log("Rotate object is inactive");
            // Move the button to the left by moveDistance units
            MoveButtonLeft();
            hasMoved = true; // Set the flag to true to indicate movement
        }
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Endless");
    }

    // Method to move the button to the left
    private void MoveButtonLeft()
    {
        Vector3 newPosition = buttonRectTransform.anchoredPosition;
        newPosition.x -= moveDistance;
        buttonRectTransform.anchoredPosition = newPosition;
    }
}
