using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelsButtonHandler : MonoBehaviour
{
    public Image backgroundOne;
    public Image backgroundTwo;
    public Image backgroundThree;
    public Image backgroundFour;
    public Image backgroundFive;
    public Image backgroundSix;
    public Image backgroundSeven;

    public Color newColor; // Color to change backgrounds to
    private float fadeDuration = 0.125f; // Duration of the fade in seconds

    public MoveDiagonally endless; // Reference to the MoveDiagonally script
    public MoveDiagonally levels; // Reference to the MoveDiagonally script
    public MoveDiagonally store; // Reference to the MoveDiagonally script
    public MoveDiagonally exit; // Reference to the MoveDiagonally script
    public MoveByX moveByX; // Reference to the MoveByX script
    public Death death; // Reference to the Death script

    public void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        AudioManager.Instance.FadeOutBackgroundMusic(1f);
        StartCoroutine(FadeBackgroundsTo(0.25f));
        StartCoroutine(exit.MoveLeftDiagonallyInitiallyCoroutine());
        StartCoroutine(WaitAndMove(0.1f, store));
        StartCoroutine(WaitAndMove(0.2f, levels));
        StartCoroutine(WaitAndMove(0.3f, endless));
        moveByX.MoveDown(5f);
        death.CloseMenu(0.5f); // Close the menu       
        StartCoroutine(LoadScene(1f)); // Wait 4 seconds, then load the scene "Endless"
    }

    // Coroutine to wait for a specified time and then move the object diagonally left
    IEnumerator WaitAndMove(float delay, MoveDiagonally moveDiagonally)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        StartCoroutine(moveDiagonally.MoveLeftDiagonallyInitiallyCoroutine()); // Start moving the object diagonally left
    }

    // Coroutine to wait for a specified time and then load the scene "Endless"
    IEnumerator LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelection");
    }

    private IEnumerator FadeBackgroundsTo(float targetAlpha)
    {
        // Store original alpha values
        float originalAlphaOne = backgroundOne.color.a;
        float originalAlphaTwo = backgroundTwo.color.a;
        float originalAlphaThree = backgroundThree.color.a;
        float originalAlphaFour = backgroundFour.color.a;
        float originalAlphaFive = backgroundFive.color.a;
        float originalAlphaSix = backgroundSix.color.a;
        float originalAlphaSeven = backgroundSeven.color.a;

        // Calculate the fade step based on the fadeDuration
        float fadeSpeed = Mathf.Abs(targetAlpha - originalAlphaOne) / fadeDuration;

        // Fade to target alpha
        while (backgroundOne.color.a > targetAlpha ||
               backgroundTwo.color.a > targetAlpha ||
               backgroundThree.color.a > targetAlpha ||
               backgroundFour.color.a > targetAlpha ||
               backgroundFive.color.a > targetAlpha ||
               backgroundSix.color.a > targetAlpha ||
               backgroundSeven.color.a > targetAlpha)
        {
            Color newColorOne = backgroundOne.color;
            newColorOne.a = Mathf.MoveTowards(newColorOne.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundOne.color = newColorOne;

            Color newColorTwo = backgroundTwo.color;
            newColorTwo.a = Mathf.MoveTowards(newColorTwo.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundTwo.color = newColorTwo;

            Color newColorThree = backgroundThree.color;
            newColorThree.a = Mathf.MoveTowards(newColorThree.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundThree.color = newColorThree;

            Color newColorFour = backgroundFour.color;
            newColorFour.a = Mathf.MoveTowards(newColorFour.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundFour.color = newColorFour;

            Color newColorFive = backgroundFive.color;
            newColorFive.a = Mathf.MoveTowards(newColorFive.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundFive.color = newColorFive;

            Color newColorSix = backgroundSix.color;
            newColorSix.a = Mathf.MoveTowards(newColorSix.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundSix.color = newColorSix;

            Color newColorSeven = backgroundSeven.color;
            newColorSeven.a = Mathf.MoveTowards(newColorSeven.a, targetAlpha, fadeSpeed * Time.deltaTime);
            backgroundSeven.color = newColorSeven;

            yield return null;
        }

        // After reaching the target alpha, fade back to original alpha
        yield return new WaitForSeconds(0.125f); // Optional delay before fading back

        float fadeBackSpeed = Mathf.Abs(originalAlphaOne - targetAlpha) / fadeDuration;

        while (backgroundOne.color.a < originalAlphaOne ||
               backgroundTwo.color.a < originalAlphaTwo ||
               backgroundThree.color.a < originalAlphaThree ||
               backgroundFour.color.a < originalAlphaFour ||
               backgroundFive.color.a < originalAlphaFive ||
               backgroundSix.color.a < originalAlphaSix ||
               backgroundSeven.color.a < originalAlphaSeven)
        {
            Color newColorOne = backgroundOne.color;
            newColorOne.a = Mathf.MoveTowards(newColorOne.a, originalAlphaOne, fadeBackSpeed * Time.deltaTime);
            backgroundOne.color = newColorOne;

            Color newColorTwo = backgroundTwo.color;
            newColorTwo.a = Mathf.MoveTowards(newColorTwo.a, originalAlphaTwo, fadeBackSpeed * Time.deltaTime);
            backgroundTwo.color = newColorTwo;

            Color newColorThree = backgroundThree.color;
            newColorThree.a = Mathf.MoveTowards(newColorThree.a, originalAlphaThree, fadeBackSpeed * Time.deltaTime);
            backgroundThree.color = newColorThree;

            Color newColorFour = backgroundFour.color;
            newColorFour.a = Mathf.MoveTowards(newColorFour.a, originalAlphaFour, fadeBackSpeed * Time.deltaTime);
            backgroundFour.color = newColorFour;

            Color newColorFive = backgroundFive.color;
            newColorFive.a = Mathf.MoveTowards(newColorFive.a, originalAlphaFive, fadeBackSpeed * Time.deltaTime);
            backgroundFive.color = newColorFive;

            Color newColorSix = backgroundSix.color;
            newColorSix.a = Mathf.MoveTowards(newColorSix.a, originalAlphaSix, fadeBackSpeed * Time.deltaTime);
            backgroundSix.color = newColorSix;

            Color newColorSeven = backgroundSeven.color;
            newColorSeven.a = Mathf.MoveTowards(newColorSeven.a, originalAlphaSeven, fadeBackSpeed * Time.deltaTime);
            backgroundSeven.color = newColorSeven;

            yield return null;
        }
    }
}
