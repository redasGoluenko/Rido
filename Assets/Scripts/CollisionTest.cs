using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D collision)
    {      
       Debug.Log("Collision Detected with " + collision.gameObject.name);
    }
}
