using UnityEngine;

public class ScaleObjectOnTouch : MonoBehaviour
{
    public float scaleAmount = 1.5f; // Factor to scale up when screen is touched
    public float transitionSpeed = 1f; // Speed at which the object scales up and down
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isScalingUp = false;
    private bool isScalingDown = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x, originalScale.y * scaleAmount, originalScale.z);
    }

    void Update()
    {
        if (Input.touchCount > 0) // Check for touch input
        {
            transform.localScale = originalScale;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isScalingUp = true;
                isScalingDown = false;
            }
        }

        if (isScalingUp)
        {
            // Smoothly scale to target scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, transitionSpeed * Time.deltaTime);

            // Check if the object has reached the target scale
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                isScalingUp = false;
                isScalingDown = true;
            }
        }

        if (isScalingDown)
        {
            // Smoothly return to original scale
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, transitionSpeed * Time.deltaTime);

            // Check if the object has reached the original scale
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isScalingDown = false;
            }
        }
    }
}
