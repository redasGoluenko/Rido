using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginButtonHandler : MonoBehaviour
{
    public Vector2 initialOffset; // The initial offset from the original position to be off-screen
    public float moveUpAmount;    // The amount to move up when the condition is met
    public float speed = 5f;      // Speed of the movement

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private RectTransform rectTransform;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rectTransform != null)
        {
            initialPosition = rectTransform.anchoredPosition;
            targetPosition = initialPosition + new Vector2(0, moveUpAmount);
            rectTransform.anchoredPosition = initialPosition + initialOffset; // Start off-screen
        }
        else if (spriteRenderer != null)
        {
            initialPosition = spriteRenderer.transform.position;
            targetPosition = initialPosition + new Vector2(0, moveUpAmount);
            spriteRenderer.transform.position = initialPosition + initialOffset; // Start off-screen
        }
        else
        {
            Debug.LogError("No RectTransform or SpriteRenderer found on the GameObject.");
        }
    }

    void Update()
    {
        if (PlayerManager.instance.levelSelected == 1)
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * speed);
            }
            else if (spriteRenderer != null)
            {
                spriteRenderer.transform.position = Vector2.Lerp(spriteRenderer.transform.position, targetPosition, Time.deltaTime * speed);
            }
        }
        else
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, initialPosition + initialOffset, Time.deltaTime * (speed * 2));
            }
            else if (spriteRenderer != null)
            {
                spriteRenderer.transform.position = Vector2.Lerp(spriteRenderer.transform.position, initialPosition + initialOffset, Time.deltaTime * (speed * 2));
            }
        }
    }
}
