using System.Collections;
using UnityEngine;

public class RescaleSprite : MonoBehaviour
{
    // Define the minimum and maximum scale values
    public Vector2 minScale = new Vector2(0.5f, 0.5f);
    public Vector2 maxScale = new Vector2(1.5f, 1.5f);

    // Speed at which the object will rescale
    public float scaleSpeed = 1.0f;
    public float delay = 0f;

    // Toggle to reverse the scaling direction
    private bool scalingUp = true;
    private bool start = false;


    private void Start()
    {
        StartCoroutine(Wait(delay));
    }

    void Update()
    {
        if (start)
        {
            Rescale();
        }           
    }

    void Rescale()
    {
        // Determine the target scale based on the scaling direction
        Vector2 targetScale = scalingUp ? maxScale : minScale;

        // Smoothly interpolate the scale of the object towards the target scale
        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

        // Check if the object is close enough to the target scale to reverse direction
        if (Vector2.Distance(transform.localScale, targetScale) < 0.01f)
        {
            scalingUp = !scalingUp;
        }
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        start = true;

    }
}
