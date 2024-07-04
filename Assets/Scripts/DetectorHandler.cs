using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorHandler : MonoBehaviour
{
    public Rotate rotate;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pivot"))
        {
            rotate.clockwise = !rotate.clockwise;
        }
    }
}
