using UnityEngine;

public class MimicMovement : MonoBehaviour
{
    public GameObject objectToFollow;

    private Vector3 initialOffset;

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
            transform.position = objectToFollow.transform.position - initialOffset;
        }
    }
}
