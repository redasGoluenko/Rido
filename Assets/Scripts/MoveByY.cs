using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class MoveByY : MonoBehaviour
{
    public ReturnButtonHandler returnButtonHandler;

    public float speed = 2.0f;
    public float distance = 2.0f;
    public void Start()
    {
        StartCoroutine(Move(Vector3.left * distance));
    }
    private IEnumerator Move(Vector3 direction)
    {      
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
    private void Retract()
    {
       StartCoroutine(Move(Vector3.right * distance));
    }
    private void Update()
    {
        if (returnButtonHandler.retract)
        {
            StartCoroutine(Move(Vector3.right * distance));
        }
    }
}
