using UnityEngine;

public class MimicMovement : MonoBehaviour
{
    public GameObject objectToFollow; // The GameObject whose movement will be mimicked

    private Vector3 initialOffset; // Offset between initial positions of this GameObject and objectToFollow

    void Start()
    {
        if (objectToFollow != null)
        {
            initialOffset = objectToFollow.transform.position - transform.position;
        }
        else
        {
            Debug.LogWarning("Object to follow is not assigned!");
        }
    }

    void Update()
    {
        if (objectToFollow != null)
        {
            // Match position relative to initial offset
            transform.position = objectToFollow.transform.position - initialOffset;

            // Match rotation if needed
            // transform.rotation = objectToFollow.transform.rotation;

            // Match other transformations as necessary (e.g., scale)
            // transform.localScale = objectToFollow.transform.localScale;
        }
    }
}
