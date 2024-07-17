using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target; // Reference to the object we want to follow

    public float followSpeed = 1f; // Speed at which the follower moves towards the target
    public float stopDistance = 0.1f; // Distance at which the follower considers it has reached the target
  
    void Update()
    {
        if (target != null)
        {
            // Move towards the target's position at a specified speed
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, followSpeed * Time.deltaTime);

            // Check if the follower has reached the target
            if (Vector3.Distance(transform.position, target.transform.position) < stopDistance)
            {
                // Destroy the follower object
                Destroy(gameObject);
            }
        }
    }
}
