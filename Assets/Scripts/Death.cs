using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Death : MonoBehaviour
{
    public Rotate rotate; // Reference to the Rotate script
    public GameObject topSlope; // Reference to the top slope object
    public GameObject bottomSlope; // Reference to the bottom slope object
    public GameObject topLining; // Reference to the top lining object
    public GameObject bottomLining; // Reference to the bottom lining object
    public GameObject leftLining; // Reference to the left lining object
    public GameObject rightLining; // Reference to the right lining object
    public GameObject menuText; // Reference to the menu text object
    public GameObject retryText; // Reference to the retry text object
    public GameObject xpText; // Reference to the xp text object
    public MoveDiagonally topSlopeScript; // Reference to the MoveDiagonally script
    public MoveDiagonally bottomSlopeScript; // Reference to the MoveDiagonally script

    private bool doOnce = true; // Flag to prevent multiple calls
    private bool isMoving = false; // Flag to track if the movement has started
    public float fadeDuration = 0.5f; // Duration of fading in and out
    private float alpha = 0f; // Initial alpha value for the text
    public float topSlopeUp = 6.47f; // Top slope up position
    public float topSlopeDown = 5.6f; // Top slope down position
    public float bottomSlopeUp = 6.5f; // Bottom slope up position
    public float bottomSlopeDown = 6.5f; // Bottom slope down position

    // Start is called before the first frame update
    void Start()
    {           
        StartCoroutine(MoveObjectInDirection(topSlope, Vector3.up, topSlopeUp, 0.25f));
        StartCoroutine(MoveObjectInDirection(bottomSlope, Vector3.down, bottomSlopeUp, 0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotate.isMenu) { HandleDeath(); }
        if (!rotate.isMenu) { HandleLiningColors(); }
    }  


    void HandleLiningColors()
    {
        if (rotate.pastNinety)
        {
            topLining.GetComponent<SpriteRenderer>().color = Color.red;
            bottomLining.GetComponent<SpriteRenderer>().color = Color.red;
            leftLining.GetComponent<SpriteRenderer>().color = Color.red;
            rightLining.GetComponent<SpriteRenderer>().color = Color.red;
            menuText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, alpha);
            retryText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, alpha);
            xpText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, alpha);
            
        }
        else if(rotate.pastSixty)
        {
            topLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            bottomLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            leftLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            rightLining.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0.5f);
            menuText.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0, 0.5f, alpha);
            retryText.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0, 0.5f, alpha);
            xpText.GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0, 0.5f, alpha);
        }
        else if(rotate.pastThirty)
        {
            topLining.GetComponent<SpriteRenderer>().color = Color.blue;
            bottomLining.GetComponent<SpriteRenderer>().color = Color.blue;
            leftLining.GetComponent<SpriteRenderer>().color = Color.blue;
            rightLining.GetComponent<SpriteRenderer>().color = Color.blue;
            menuText.GetComponent<TextMeshProUGUI>().color = new Color(0.0f, 0.0f, 1.0f, alpha);
            retryText.GetComponent<TextMeshProUGUI>().color = new Color(0.0f, 0.0f, 1.0f, alpha);
            xpText.GetComponent<TextMeshProUGUI>().color = new Color(0.0f, 0.0f, 1.0f, alpha);
        }
        else
        {
            topLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            bottomLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            leftLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            rightLining.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.92f, 0.3f);
            menuText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.92f, 0.3f, alpha);
            retryText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.92f, 0.3f, alpha);
            xpText.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.92f, 0.3f, alpha);
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
            StartCoroutine(MoveObjectInDirection(topSlope, topSlope.transform.up * -1, topSlopeDown, 0.5f));       
            StartCoroutine(MoveObjectInDirection(bottomSlope, bottomSlope.transform.up, bottomSlopeDown, 0.5f));
            StartCoroutine(ChangeAlphaOverTime(1.0f, fadeDuration)); // Fade out the text

        }
    }  
    public void CloseMenu()
    {
        Debug.Log("Menu Closed");
        topSlopeScript.slopeMovement = false;
        bottomSlopeScript.slopeMovement = false;
        StartCoroutine(WaitAndClose(0.5f));
    }
    IEnumerator WaitAndClose(float duration)
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(MoveObjectInDirection(topSlope, topSlope.transform.up * -1, topSlopeDown, 0.5f));
        StartCoroutine(MoveObjectInDirection(bottomSlope, bottomSlope.transform.up, bottomSlopeDown, 0.5f));
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
    IEnumerator ChangeAlphaOverTime(float targetAlpha, float duration)
    {
        float startAlpha = alpha; // Store the initial alpha value
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate between startAlpha and targetAlpha over time
            alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            // Increment elapsedTime
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure we reach exactly the target alpha
        alpha = targetAlpha;
    }
}
