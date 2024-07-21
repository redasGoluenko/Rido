using UnityEngine;
using TMPro;
using System.Collections;

public class FadeTextMeshPro : MonoBehaviour
{
    public bool isVisible; // Boolean variable to control visibility
    public float fadeDuration = 1f; // Duration for fade in and out
    private TextMeshProUGUI textMeshPro; // Use TextMeshProUGUI for UI TextMeshPro

    private void Start()
    {
        isVisible = PlayerManager.instance.levelSelected == 1;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the GameObject.");
            return;
        }

        // Initialize the alpha based on the starting visibility state
        SetAlpha(isVisible ? 1f : 0f);
    }

    private void Update()
    {
        isVisible = PlayerManager.instance.levelSelected == 1;
        if (textMeshPro == null) return;

        // Check if the visibility state has changed
        bool currentVisibility = textMeshPro.color.a > 0.5f; // Rough check if visible
        if (currentVisibility != isVisible)
        {
            StopAllCoroutines(); // Stop any ongoing fade
            StartCoroutine(FadeText(isVisible ? 1f : 0f)); // Start new fade coroutine
        }
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        float startAlpha = textMeshPro.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        // Ensure the final alpha value is set correctly
        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = textMeshPro.color;
        color.a = alpha;
        textMeshPro.color = color;
    }
}
