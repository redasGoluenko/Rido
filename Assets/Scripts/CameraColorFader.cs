//Purpose: cycles through background colors in a smooth transition effect

using UnityEngine;
using System.Collections;
using System.Net;

public class CameraColorFader : MonoBehaviour
{    
    private Color pastelYellow = new Color(1f, 0.96f, 0.7f);
    private Color pastelRed = new Color(1f, 0.6f, 0.6f);
    private Color pastelBlue = new Color(0.7f, 0.85f, 1f);
    private Color pastelPurple = new Color(0.85f, 0.7f, 1f);
    private Color[] colors;   
    public float transitionDuration = 2.0f; 
    private int currentColorIndex = 0;
    private float transitionProgress = 0f;
    private Camera mainCamera;
    public bool pause = false;

    void Start()
    {      
        colors = new Color[] { pastelRed, pastelBlue, pastelPurple, pastelYellow };
     
        mainCamera = GetComponent<Camera>();
      
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = colors[currentColorIndex];
        }
    }

    void Update()
    {       
        if (pause || mainCamera == null)
        {
            return;
        }
        else
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            if (transitionProgress >= 1f)
            {
                transitionProgress = 0f;
                currentColorIndex = (currentColorIndex + 1) % colors.Length;
            }
            int nextColorIndex = (currentColorIndex + 1) % colors.Length;
            mainCamera.backgroundColor = Color.Lerp(colors[currentColorIndex], colors[nextColorIndex], transitionProgress);
        }       
    }
    // Method to start the color fade
    public void ChangeColor(Color targetColor)
    {
        StartCoroutine(FadeToColor(targetColor));
    }

    // Coroutine to fade to the target color
    private IEnumerator FadeToColor(Color targetColor)
    {
        Color startColor = mainCamera.backgroundColor;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            // Calculate the interpolated color
            mainCamera.backgroundColor = Color.Lerp(startColor, targetColor, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the final color is set
        mainCamera.backgroundColor = targetColor;
    }
}
