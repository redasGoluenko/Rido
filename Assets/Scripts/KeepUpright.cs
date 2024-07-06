using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    void Update()
    {
        // Ensure the sprite stays upright relative to the camera's up direction
        Camera mainCamera = Camera.main;
        Vector3 cameraUp = mainCamera.transform.up;
        transform.up = cameraUp;
    }
}
