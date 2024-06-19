using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCollider : MonoBehaviour
{
    public bool Available = false;
    public bool PlayerColliding = false;

    // Called when this collider/rigidbody has begun touching another rigidbody/collider.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pivot")
        {
            Available = true;
        }
        if (collision.gameObject.tag == "Player")
        {
            PlayerColliding = true;
        }
    }

    // Called when this collider/rigidbody has stopped touching another rigidbody/collider.
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pivot")
        {
            Available = false;
        }     
        if (collision.gameObject.tag == "Player")
        {
            PlayerColliding = false;
        }
    }
}

