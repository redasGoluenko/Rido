using UnityEngine;

public class ChangeTrailAlpha : MonoBehaviour
{
    // Reference to the TrailRenderer component
    private TrailRenderer trailRenderer;

    // The desired alpha value
    public float targetAlpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the TrailRenderer component attached to this GameObject
        trailRenderer = GetComponent<TrailRenderer>();

        // Check if the TrailRenderer component exists
        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change the alpha of the trail renderer material
        ChangeAlpha();
    }

    void ChangeAlpha()
    {
        // Get the current material used by the TrailRenderer
        Material trailMaterial = trailRenderer.material;

        // Ensure that the material has the property for alpha (assuming it uses Standard shader)
        if (trailMaterial.HasProperty("_Color"))
        {
            // Get the current color
            Color currentColor = trailMaterial.GetColor("_Color");

            // Set the alpha value to the target alpha
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

            // Apply the new color to the material
            trailMaterial.SetColor("_Color", newColor);
        }
        else
        {
            Debug.LogWarning("TrailRenderer material does not have _Color property.");
        }
    }
}
