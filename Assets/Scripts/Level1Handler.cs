using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Handler : MonoBehaviour
{
    private CameraColorFader cameraColorFader;
    private List<CycleLiningColors> cycleLiningColorsObjects;
    private bool shouldPause;
    // Start is called before the first frame update
    void Start()
    {
        cameraColorFader = GameObject.Find("Main Camera").GetComponent<CameraColorFader>();       
        CycleLiningColors[] cycleLiningColorsArray = FindObjectsByType<CycleLiningColors>(FindObjectsSortMode.None);
        cycleLiningColorsObjects = new List<CycleLiningColors>(cycleLiningColorsArray);               
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player selected level 1
        shouldPause = PlayerManager.instance.levelSelected == 1;
        cameraColorFader.pause = shouldPause;
        if (shouldPause)
        {
            cameraColorFader.ChangeColor(Color.black);
        }       
        foreach (var obj in cycleLiningColorsObjects)
        {
            if (obj != null)
            {
                obj.pause = shouldPause;
                if(shouldPause)
                {
                    obj.ChangeColor(Color.white);
                }
            }
        }        
        
    }
}
