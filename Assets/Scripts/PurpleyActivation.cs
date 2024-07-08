using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleyActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("glowNumber") != 4)
        {
            gameObject.SetActive(false);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
