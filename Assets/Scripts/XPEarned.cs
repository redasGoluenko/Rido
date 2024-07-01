using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPEarned : MonoBehaviour
{  
    //textmeshpro object
    public TextMeshProUGUI scoreText;
    public int xpEarned = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"XP Earned: {xpEarned}";
    }
}
