//Purpose: Handler for Bluey's collision interactions with tokens and the environment

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueyHandler : MonoBehaviour
{  
    private Coroutine fadeCoroutine;

    public GameObject leftPupil;
    public GameObject rightPupil;   

    public GameObject triangleOne;
    public GameObject triangleTwo;
    public GameObject triangleThree;
    public GameObject triangleFour;  

    private int glowNumber;
    private bool inGlowSelection = false;

    private void Start()
    {                   
        // Used to avoid unnecessary calculations when not in the Glows scene
        if(SceneManager.GetActiveScene().name == "Glows")
        {
            inGlowSelection = true;
            
        }     
        else
        {
            glowNumber = PlayerManager.instance.glowNumber;
        }
    }
    void Update()
    {      
        // If in the Glows scene, update the glow number from the PlayerManager
        if (inGlowSelection)
        {
            glowNumber = PlayerManager.instance.glowNumber;
        }

        HandleBlueyGlow();       
    }

    // Handles the glow effect for Bluey
    void HandleBlueyGlow()
    {
        if (glowNumber == 3 && Input.touchCount > 0)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            SetAlpha(1f);
            fadeCoroutine = StartCoroutine(FadeOut(0.75f));
        }
        else
        {
            SetAlpha(0f);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.CompareTag("Token"))
        {                               
            leftPupil.GetComponent<SpriteRenderer>().color = Color.yellow;
            rightPupil.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (collision.gameObject.CompareTag("RedirectToken"))
        {
            leftPupil.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 162f / 255f, 255f / 255f);
            rightPupil.GetComponent<SpriteRenderer>().color = new Color(0f / 255f, 162f / 255f, 255f / 255f);
        }
        else if (collision.gameObject.CompareTag("HoldToken"))
        {             
            leftPupil.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0f / 255f, 255f / 255f);
            rightPupil.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (collision.gameObject.CompareTag("RedToken"))
        {
            leftPupil.GetComponent<SpriteRenderer>().color = Color.red;
            rightPupil.GetComponent<SpriteRenderer>().color = Color.red;      
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Token"))
        {                  
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        else if (collision.gameObject.CompareTag("RedirectToken"))
        {                     
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }
        else if (collision.gameObject.CompareTag("HoldToken"))
        {                 
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
            SetScale(Vector3.one);          
        }
        else if (collision.gameObject.CompareTag("RedToken"))
        {           
            if (fadeCoroutine == null)
            {
                SetAlpha(0f);
            }
        }        
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0f;
        float startAlpha = 1f;
        Vector3 initialScale = Vector3.one;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            SetAlpha(newAlpha);           
            Vector3 newScale = initialScale * (1 + (newAlpha / 4));
            SetScale(newScale);

            yield return null;
        }
       
        SetAlpha(0f);
        SetScale(initialScale);
        fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {        
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
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
