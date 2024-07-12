using System.Collections;
using UnityEngine;

public class ScaleObjectOnTouch : MonoBehaviour
{
    public float scaleAmount = 1.5f; // Factor to scale up when the screen is touched
    public float transitionSpeed = 1f; // Speed at which the object scales up and down
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isScalingUp = false;
    private bool isScalingDown = false;
    public GameObject leftPupil;
    public GameObject rightPupil;

    private Vector3 originalLeftPupilScale;
    private Vector3 originalRightPupilScale;
    private Vector3 targetLeftPupilScale;
    private Vector3 targetRightPupilScale;
 

    void Start()
    {     
        // Store the original scales
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x, originalScale.y * scaleAmount, originalScale.z);

        originalLeftPupilScale = leftPupil.transform.localScale;
        originalRightPupilScale = rightPupil.transform.localScale;

        // Set the target scales for the pupils (only x-axis is increased)
        targetLeftPupilScale = new Vector3(originalLeftPupilScale.x * 2, originalLeftPupilScale.y, originalLeftPupilScale.z);
        targetRightPupilScale = new Vector3(originalRightPupilScale.x * 2, originalRightPupilScale.y, originalRightPupilScale.z);
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
            // Smoothly scale the main object to the target scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, transitionSpeed * Time.deltaTime);

            // Smoothly scale the pupils to their target scales
            leftPupil.transform.localScale = Vector3.Lerp(leftPupil.transform.localScale, targetLeftPupilScale, transitionSpeed * Time.deltaTime);
            rightPupil.transform.localScale = Vector3.Lerp(rightPupil.transform.localScale, targetRightPupilScale, transitionSpeed * Time.deltaTime);

            // Check if the main object and pupils have reached their target scales
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f &&
                Vector3.Distance(leftPupil.transform.localScale, targetLeftPupilScale) < 0.01f &&
                Vector3.Distance(rightPupil.transform.localScale, targetRightPupilScale) < 0.01f)
            {
                transform.localScale = targetScale;
                leftPupil.transform.localScale = targetLeftPupilScale;
                rightPupil.transform.localScale = targetRightPupilScale;
                isScalingUp = false;
                isScalingDown = true;
            }
        }

        if (isScalingDown)
        {
            // Smoothly return the main object to its original scale
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, transitionSpeed * Time.deltaTime);

            // Smoothly return the pupils to their original scales
            leftPupil.transform.localScale = Vector3.Lerp(leftPupil.transform.localScale, originalLeftPupilScale, transitionSpeed * Time.deltaTime);
            rightPupil.transform.localScale = Vector3.Lerp(rightPupil.transform.localScale, originalRightPupilScale, transitionSpeed * Time.deltaTime);

            // Check if the main object and pupils have returned to their original scales
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f &&
                Vector3.Distance(leftPupil.transform.localScale, originalLeftPupilScale) < 0.01f &&
                Vector3.Distance(rightPupil.transform.localScale, originalRightPupilScale) < 0.01f)
            {
                transform.localScale = originalScale;
                leftPupil.transform.localScale = originalLeftPupilScale;
                rightPupil.transform.localScale = originalRightPupilScale;
                isScalingDown = false;
            }
        }
    }  
}
