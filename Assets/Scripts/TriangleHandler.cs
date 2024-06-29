using UnityEngine;

public class TriangleHandler : MonoBehaviour
{
    public float scaleFactor = 0.10f;
    public float scaleSpeed = 5.0f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isCollidingWithRotationCenter = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        // Check initial collision status with RotationCenter
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("RotationCenter"))
            {
                isCollidingWithRotationCenter = true;
                targetScale = originalScale + new Vector3(scaleFactor + 0.4f, scaleFactor + 0.3f, 0);
                break;
            }
        }
    }

    void Update()
    {
        // Smoothly interpolate towards the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Detector"))
        {
            // Increase the target scale only if not colliding with RotationCenter
            if (!isCollidingWithRotationCenter)
            {
                targetScale = originalScale + new Vector3(scaleFactor + 0.2f, scaleFactor, 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RotationCenter"))
        {
            isCollidingWithRotationCenter = true;
            targetScale = originalScale + new Vector3(scaleFactor + 0.4f, scaleFactor + 0.3f, 0);
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
}
