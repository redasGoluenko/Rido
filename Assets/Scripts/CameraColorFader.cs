using UnityEngine;

public class CameraColorFader : MonoBehaviour
{
    // Define the pastel colors
    private Color pastelYellow = new Color(1f, 0.96f, 0.7f);
    private Color pastelRed = new Color(1f, 0.6f, 0.6f);
    private Color pastelBlue = new Color(0.7f, 0.85f, 1f);
    private Color pastelPurple = new Color(0.85f, 0.7f, 1f);

    // Array to hold the colors for easy access
    private Color[] colors;

    // Duration for each color transition
    public float transitionDuration = 2.0f;

    // Private variables to track state
    private int currentColorIndex = 0;
    private float transitionProgress = 0f;
    private Camera mainCamera;

    void Start()
    {
        // Initialize the colors array
        colors = new Color[] { pastelYellow, pastelRed, pastelBlue, pastelPurple };

        // Get the Camera component
        mainCamera = GetComponent<Camera>();

        // Set the initial background color
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = colors[currentColorIndex];
        }
    }

    void Update()
    {
        // Ensure the Camera component exists
        if (mainCamera == null) return;

        // Update the transition progress
        transitionProgress += Time.deltaTime / transitionDuration;

        // If the transition is complete, move to the next color
        if (transitionProgress >= 1f)
        {
            transitionProgress = 0f;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }

        // Calculate the next color index
        int nextColorIndex = (currentColorIndex + 1) % colors.Length;

        // Lerp between the current color and the next color
        mainCamera.backgroundColor = Color.Lerp(colors[currentColorIndex], colors[nextColorIndex], transitionProgress);
    }
}
