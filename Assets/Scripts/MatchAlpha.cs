using UnityEngine;

public class MatchAlphaToTarget : MonoBehaviour
{
    // Reference to the target GameObject whose alpha will be matched
    public GameObject targetObject;

    private SpriteRenderer targetSpriteRenderer;
    private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer components
        if (targetObject != null)
        {
            targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (targetSpriteRenderer != null && mySpriteRenderer != null)
        {
            // Get the alpha of the target object
            float targetAlpha = targetSpriteRenderer.color.a;

            // Get the current color of this object
            Color myColor = mySpriteRenderer.color;

            // Set the alpha of this object's color to match the target's alpha
            myColor.a = targetAlpha;

            // Apply the updated color to this object
            if(targetAlpha == 1f)
            {
                mySpriteRenderer.color = myColor;
            }           
        }
    }
}
