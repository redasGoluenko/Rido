using UnityEngine;

public class PulsateSize : MonoBehaviour
{
    public float pulseSpeed = 2f;    // Speed of the pulsating effect
    public float maxScale = 1.5f;    // Maximum scale factor
    public float minScale = 0.5f;    // Minimum scale factor

    private Vector3 originalScale;

    void Start()
    {
        // Store the original scale of the object
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Calculate the scale factor based on time
        float lerpTime = Mathf.PingPong(Time.time * pulseSpeed, 1);
        float scaleFactor = Mathf.Lerp(minScale, maxScale, lerpTime);

        // Apply the scale factor to the original scale
        transform.localScale = originalScale * scaleFactor;
    }
}
