using UnityEngine;

public class SpinObject2D : MonoBehaviour
{
    public float spinSpeed = 200f; // Speed of rotation in degrees per second
    public bool clockwise = true; // Direction of rotation

    void Update()
    {
        if (clockwise)
        {
            // Rotate the object around its z-axis (usually the forward axis in 2D)
            transform.Rotate(Vector3.forward, -spinSpeed * Time.deltaTime);           
        }
        else
        {
            // Rotate the object around its z-axis (usually the forward axis in 2D)
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
        }     
    }
}
