using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public Vector2 initialOffset; // The initial offset from the original position to be off-screen
    public float moveUpAmount;    // The amount to move up when the condition is met
    public float speed = 5f;      // Speed of the movement

    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        targetPosition = initialPosition + new Vector2(0, moveUpAmount);
        rectTransform.anchoredPosition = initialPosition + initialOffset; // Start off-screen
    }

    void Update()
    {
        if (PlayerManager.instance.levelSelected == 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * speed);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, initialPosition + initialOffset, Time.deltaTime * (speed * 2));
        }
    }
}
