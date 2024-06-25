using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleSlopeColliding : MonoBehaviour
{
    public bool isCollidingWithSlope = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {
            Debug.Log("Collided with slope");
            isCollidingWithSlope = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {
            Debug.Log("Exited slope");
            isCollidingWithSlope = false;
        }
    }
}
