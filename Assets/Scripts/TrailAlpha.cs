using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailAlpha : MonoBehaviour
{
    private TrailRenderer trail;
    public float alphaValue = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.material.color = new Color(1, 1, 1, alphaValue);       
    }
}
