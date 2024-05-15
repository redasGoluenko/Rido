using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Transform UpperPivot;
    public Transform LowerPivot;

    public float rotationSpeed = 10f;
    public float speedIncrementAmount = 1000f;
    public float speedIncrementInterval = 0.25f;

    private bool isTouching = false;

    //on collision with border destroy
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Border")
        { 
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void Start()
    {
        // Disable renderers of upper and lower pivots
        if (UpperPivot != null)
            UpperPivot.GetComponent<Renderer>().enabled = false;

        if (LowerPivot != null)
            LowerPivot.GetComponent<Renderer>().enabled = false;

        // Start the coroutine to increment speed
        StartCoroutine(IncrementSpeedCoroutine());
    }

    IEnumerator IncrementSpeedCoroutine()
    {
        // Run this coroutine indefinitely
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(speedIncrementInterval);

            // Increment the rotation speed
            rotationSpeed += speedIncrementAmount;
        }
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            isTouching = true;
        }
        else
        {
            isTouching = false;
        }

        // Rotate around the appropriate pivot
        if (isTouching && LowerPivot != null)
        {
            RotateAroundPivot(LowerPivot, Vector3.back);
        }
        else if (UpperPivot != null)
        {
            RotateAroundPivot(UpperPivot, Vector3.forward);
        }
    }

    // Rotate around the given pivot point with a specific axis
    private void RotateAroundPivot(Transform pivot, Vector3 axis)
    {
        // Calculate the direction vector from the pivot to this object
        Vector3 direction = pivot.position - transform.position;

        // Rotate the object around the pivot
        transform.RotateAround(pivot.position, axis, rotationSpeed * Time.deltaTime);
    }

    public void IncrementSpeed(float amount)
    {
        rotationSpeed += amount;
    }
}
