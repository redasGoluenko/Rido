using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPEarned : MonoBehaviour
{  
    //textmeshpro object
    public TextMeshProUGUI scoreText;
    public PurpleTokenCurrent purpleTokenCurrent;
    public RedTokenCurrent redTokenCurrent;
    public BlueTokenCurrent blueTokenCurrent;
    public GoldTokenCurrent goldTokenCurrent;
    public GameObject xpEarned;

    public float animationDuration = 1.0f; // Duration of the animation in seconds
    private Coroutine animateCoroutine;
    public bool isDone = false;

    private CanvasRenderer xpRenderer; // Reference to the CanvasRenderer component of goldToken

    void Start()
    {
        scoreText.text = "0"; // Initialize text

        // Get the CanvasRenderer component from the purpleToken GameObject
        xpRenderer = xpEarned.GetComponent<CanvasRenderer>();
        // Set initial alpha of purpleToken to 0 (fully transparent)
        xpRenderer.SetAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
