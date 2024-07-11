//Purpose: cycles through background colors in a smooth transition effect

using UnityEngine;

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
        if (mainCamera == null) return;     
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
