using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginButtonHandler : MonoBehaviour
{
    public ReturnButtonHandler ReturnButtonHandler;
    public RectTransform returnRect;
    public RectTransform beginRect;
    public RectTransform backgroundRect;
    public RectTransform liningRect;
    public RectTransform centerRect;
    public Camera cam;
    public SpriteRenderer eyeball;
    public float moveDistance = 100f; // Distance to move the rects down
    public float moveDuration = 0.5f; // Duration of the move
    public float cameraMoveDuration = 0.5f; // Duration of the camera move

    public void OnButtonClick()
    {
        ReturnButtonHandler.retract = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        if (PlayerManager.instance.levelSelected == 1)
        {
            Debug.Log("Button Clicked!");
        }

        // Start the coroutine to move the rects down
        StartCoroutine(MoveRectsDown());
        StartCoroutine(CenterCameraOnEyeball());       
        StartCoroutine(WaitAndLoad(1f));
    }

    private IEnumerator MoveRectsDown()
    {
        // Store the starting and ending positions for all RectTransforms
        Vector2 returnStartPos = returnRect.anchoredPosition;
        Vector2 beginStartPos = beginRect.anchoredPosition;
        Vector2 backgroundStartPos = backgroundRect.anchoredPosition;
        Vector2 liningStartPos = liningRect.anchoredPosition;
        Vector2 centerStartPos = centerRect.anchoredPosition;

        Vector2 returnEndPos = returnStartPos + new Vector2(0, -moveDistance);
        Vector2 beginEndPos = beginStartPos + new Vector2(0, -moveDistance);
        Vector2 backgroundEndPos = backgroundStartPos + new Vector2(0, -moveDistance);
        Vector2 liningEndPos = liningStartPos + new Vector2(0, -moveDistance);
        Vector2 centerEndPos = centerStartPos + new Vector2(0, -moveDistance);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            returnRect.anchoredPosition = Vector2.Lerp(returnStartPos, returnEndPos, elapsedTime / moveDuration);
            beginRect.anchoredPosition = Vector2.Lerp(beginStartPos, beginEndPos, elapsedTime / moveDuration);
            backgroundRect.anchoredPosition = Vector2.Lerp(backgroundStartPos, backgroundEndPos, elapsedTime / moveDuration);
            liningRect.anchoredPosition = Vector2.Lerp(liningStartPos, liningEndPos, elapsedTime / moveDuration);
            centerRect.anchoredPosition = Vector2.Lerp(centerStartPos, centerEndPos, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        returnRect.anchoredPosition = returnEndPos;
        beginRect.anchoredPosition = beginEndPos;
        backgroundRect.anchoredPosition = backgroundEndPos;
        liningRect.anchoredPosition = liningEndPos;
        centerRect.anchoredPosition = centerEndPos;
    }

    private IEnumerator CenterCameraOnEyeball()
    {
        if (eyeball != null && cam != null)
        {
            Vector3 startCamPos = cam.transform.position;
            Vector3 endCamPos = new Vector3(eyeball.transform.position.x, eyeball.transform.position.y, cam.transform.position.z);

            float elapsedTime = 0f;

            while (elapsedTime < cameraMoveDuration)
            {
                cam.transform.position = Vector3.Lerp(startCamPos, endCamPos, elapsedTime / cameraMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cam.transform.position = endCamPos;
        }
        else
        {
            Debug.LogWarning("Eyeball or Camera is not set.");
        }
    }

    private IEnumerator WaitAndLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
