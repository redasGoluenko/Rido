using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private bool isColliding = false;
    private bool isCollidingHoldToken = false;
    private Coroutine fadeCoroutine;

    public GameObject leftPupil;
    public GameObject rightPupil;

    public GameObject triangleOne;
    public GameObject triangleTwo;
    public GameObject triangleThree;
    public GameObject triangleFour;

    void Update()
    {
        if (Input.touchCount > 0)
            {          
            if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                // Start the flash effect: set alpha to 1 and begin fading out            
                SetAlpha(1f);
                fadeCoroutine = StartCoroutine(FadeOut(0.75f)); // Adjust the duration as needed                       

                // Since the object will be destroyed on touch, we reset the collision state
                isColliding = false;
            }
            else if (!isColliding)
            {
                // Ensure alpha is set to 0 when not colliding or flash is not active
                SetAlpha(0f);
            }

            if (isCollidingHoldToken && Input.touchCount > 0)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                SetAlpha(1f);
            }
            else if (!isCollidingHoldToken)
            {
                SetAlpha(0f);
            }     
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.CompareTag("Token"))
        {                    
            isColliding = true;            
            leftPupil.GetComponent<SpriteRenderer>().color = Color.yellow;
            rightPupil.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            isColliding = true;
            leftPupil.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 162f / 255f, 255f / 255f);
            rightPupil.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 162f / 255f, 255f / 255f);
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            isColliding = true;
            isCollidingHoldToken = true;
            leftPupil.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0f / 255f, 255f / 255f);
            rightPupil.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0f / 255f, 255f / 255f);
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            leftPupil.GetComponent<SpriteRenderer>().color = Color.red;
            rightPupil.GetComponent<SpriteRenderer>().color = Color.red;
            isColliding = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        if (collision.gameObject.CompareTag("RedirectToken"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        if (collision.gameObject.CompareTag("HoldToken"))
        {
            isColliding = false;
            isCollidingHoldToken = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
            SetScale(Vector3.one);
        }
        if (collision.gameObject.CompareTag("RedToken"))
        {
            isColliding = false;
            // Ensure alpha is set to 0 when the collision ends and no fade is ongoing
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }        
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = 1f; // Start from fully visible since we set it to 1f on touch
        Vector3 initialScale = Vector3.one; // Initial scale of triangles

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            SetAlpha(newAlpha);

            // Scale the triangles based on alpha (example: increase size when alpha is high, decrease when fading out)
            Vector3 newScale = initialScale * (1 + (newAlpha / 4)); // Adjust scaling factor as needed
            SetScale(newScale);

            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is set to 0 at the end of the fade
        SetAlpha(0f);
        SetScale(initialScale); // Reset scale to original
        fadeCoroutine = null; // Reset the coroutine reference
    }

    private void SetAlpha(float alpha)
    {        
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) // Ensure there's a SpriteRenderer component
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }
    }

    private void SetScale(Vector3 scale)
    {
        triangleOne.transform.localScale = scale;
        triangleTwo.transform.localScale = scale;
        triangleThree.transform.localScale = scale;
        triangleFour.transform.localScale = scale;
    }

}
