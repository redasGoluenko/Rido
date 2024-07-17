using UnityEngine;

public class TriangleHandler : MonoBehaviour
{
    public float scaleFactor = 0.10f;
    public float scaleSpeed = 5.0f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isCollidingWithRotationCenter = false;

    private Renderer triangleRenderer;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        // Get the Renderer component
        triangleRenderer = GetComponent<Renderer>();

        // Check initial collision status with RotationCenter
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("RotationCenter"))
            {
                isCollidingWithRotationCenter = true;
                targetScale = originalScale + new Vector3(scaleFactor + 0.36f, scaleFactor + 0.3f, 0);
                break;
            }
        }

        // Set initial visibility
        UpdateVisibility();
    }

    void Update()
    {
        // Smoothly interpolate towards the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

        // Update the visibility based on the current scale
        UpdateVisibility();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Detector"))
        {
            // Increase the target scale only if not colliding with RotationCenter
            if (!isCollidingWithRotationCenter)
            {
                targetScale = originalScale + new Vector3(scaleFactor + 0.3f, scaleFactor, 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RotationCenter"))
        {
            isCollidingWithRotationCenter = true;
            targetScale = originalScale + new Vector3(scaleFactor + 0.36f, scaleFactor + 0.3f, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Detector"))
        {
            // Decrease the target scale only if not colliding with RotationCenter
            if (!isCollidingWithRotationCenter)
            {
                targetScale = originalScale;
            }
        }
        else if (collision.gameObject.CompareTag("RotationCenter"))
        {
            isCollidingWithRotationCenter = false;
            targetScale = originalScale;
        }
    }

    private void UpdateVisibility()
    {
        // Check if the triangle is extended beyond its original scale
        bool isExtended = transform.localScale != originalScale;

        // Set the Renderer visibility based on whether the triangle is extended
        triangleRenderer.enabled = isExtended;
    }
}
