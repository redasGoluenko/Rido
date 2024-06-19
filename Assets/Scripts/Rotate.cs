using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform rotationCenter; // Reference to the object we want to rotate around
    public float distance = 2f; // Distance from rotationCenter
    public float rotateSpeed = 50f; // Speed of rotation in degrees per second
    public bool clockwise = true; // Direction of rotation

    private Vector3 orbitPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure rotationCenter is assigned in the Inspector or find it here if it's not assigned.
        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center not assigned!");
            return;
        }

        // Calculate the initial orbit position
        orbitPosition = (transform.position - rotationCenter.position).normalized * distance + rotationCenter.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure rotationCenter is assigned in the Inspector or find it here if it's not assigned.
        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center not assigned!");
            return;
        }

        // Orbit around the rotationCenter at a certain distance
        OrbitAround();

        // Example: Call MirrorPosition() method on user input (e.g., space key press)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MirrorPosition();
        }
    }

    void OrbitAround()
    {
        // Rotate around the rotationCenter
        float direction = clockwise ? 1f : -1f;
        transform.RotateAround(rotationCenter.position, Vector3.forward, direction * rotateSpeed * Time.deltaTime);

        // Maintain the desired distance from rotationCenter
        orbitPosition = (transform.position - rotationCenter.position).normalized * distance + rotationCenter.position;
        transform.position = orbitPosition;
    }

    public void MirrorPosition()
    {
        // Calculate current relative position
        Vector3 relativePos = transform.position - rotationCenter.position;

        // Mirror position relative to rotationCenter
        Vector3 mirroredPos = new Vector3(-relativePos.x, -relativePos.y, relativePos.z);

        // Apply the mirrored position directly
        transform.position = rotationCenter.position + mirroredPos;
    }
}
