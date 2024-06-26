using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public GameObject topSlope; // Reference to the top slope object
    public GameObject bottomSlope; // Reference to the bottom slope object
    public GameObject topLining;
    public GameObject bottomLining;
    public GameObject leftLining;
    public GameObject rightLining;

    private bool doOnce = true; // Flag to prevent multiple calls
    private bool isMoving = false; // Flag to track if the movement has started

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndExecute(1f)); // Wait for a second before starting the movement
        // Initial movement in local directions
        StartCoroutine(MoveObjectInDirection(topSlope, Vector3.up, 6.47f, 1f));       
        StartCoroutine(MoveObjectInDirection(bottomSlope, Vector3.down, 5.5f, 1f));       
    }

    // Update is called once per frame
    void Update()
    {
        HandleDeath();
        HandleLiningColors();
    }

    void PreventPassingThrough()
    {      
    }

    void HandleLiningColors()
    {
        if (rotate.pastNinety)
        {
            topLining.GetComponent<SpriteRenderer>().color = Color.red;
            bottomLining.GetComponent<SpriteRenderer>().color = Color.red;
            leftLining.GetComponent<SpriteRenderer>().color = Color.red;
            rightLining.GetComponent<SpriteRenderer>().color = Color.red;

            
        }
        else if(rotate.pastSixty)
        {
            topLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            bottomLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            leftLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            rightLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
        }
        else if(rotate.pastThirty)
        {
            topLining.GetComponent<SpriteRenderer>().color = Color.blue;
            bottomLining.GetComponent<SpriteRenderer>().color = Color.blue;
            leftLining.GetComponent<SpriteRenderer>().color = Color.blue;
            rightLining.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            topLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            bottomLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            leftLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            rightLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
        }
    }
    public void HandleDeath()
    {
        // Only start the movement if 'rotate.dead' is true and movement hasn't started
        if (rotate.dead && !isMoving && doOnce)
        {
            doOnce = false; // Prevent multiple calls           
            isMoving = true; // Set the flag to prevent starting the coroutine again           

            // Move the slopes in their local directions relative to their current rotation
            StartCoroutine(MoveObjectInDirection(topSlope, topSlope.transform.up * -1, 5.6f, 1f));       
            StartCoroutine(MoveObjectInDirection(bottomSlope, bottomSlope.transform.up, 5.5f, 1f));       
        }
    }

    IEnumerator MoveObjectInDirection(GameObject obj, Vector3 direction, float distance, float duration)
    {
        if (obj == null)
        {
            yield break;
        }

        Vector3 startingPosition = obj.transform.position;
        Vector3 destination = startingPosition + direction.normalized * distance; // Calculate the new target position
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startingPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = destination; // Ensure we finish exactly at the destination

        isMoving = false; // Reset the flag if you want to allow movement again later
    }

    IEnumerator WaitAndExecute(float duration) { yield return new WaitForSeconds(duration); }
    
}
