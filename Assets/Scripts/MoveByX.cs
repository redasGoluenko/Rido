using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByX : MonoBehaviour
{   
    public float speed = 2.0f;          
    public void MoveDown(float distance)
    {
        StartCoroutine(Move(Vector3.down * distance));
    }    
    private IEnumerator Move(Vector3 direction)
    {   
        Transform originalParent = transform.parent;
        transform.SetParent(null);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction;
        float totalDistance = direction.magnitude;
        float traveledDistance = 0.0f;

        while (traveledDistance < totalDistance)
        {         
            float moveStep = speed * Time.deltaTime;          
            transform.position += direction.normalized * moveStep;        
            traveledDistance += moveStep;        
            yield return null;
        }       
        transform.position = targetPosition;
    }
}
