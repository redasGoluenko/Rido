//Purpose: bottom collider for the rotation center responsible for spawning tokens in appropriate positions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    private GameObject currentToken;

    public Rotate rotate;
    public GameObject tokenPrefab;
    public GameObject redirectTokenPrefab;
    public GameObject holdTokenPrefab;
    public GameObject redTokenPrefab;

    public bool Available = false;
    public bool PlayerColliding = false;

    private int pivotContactCount = 0;
    private int playerContactCount = 0;
   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pivot"))
        {
            pivotContactCount++;
            if (pivotContactCount == 1)
            {
                Available = true;                
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerContactCount++;
            if (playerContactCount == 1)
            {
                PlayerColliding = true;             
            }
        }
    }
  
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pivot"))
        {
            pivotContactCount--;
            if (pivotContactCount <= 0)
            {
                Available = false;             
                pivotContactCount = 0;
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            playerContactCount--;
            if (playerContactCount <= 0)
            {
                PlayerColliding = false;               
                playerContactCount = 0;
            }
        }
    }

    // Spawns Token after a standard one is picked up
    public void SpawnToken()
    {
        currentToken = RandomToken();
        if (currentToken != null)
        {           
            Vector3 spawnPosition = transform.position;            
            Vector3 downPosition = spawnPosition + Vector3.down * 1.5f;
            Vector3 leftPosition = spawnPosition + Vector3.left + Vector3.down * 0.5f;
            Vector3 rightPosition = spawnPosition + Vector3.right + Vector3.down * 0.5f;       
            List<Vector3> potentialPositions = new List<Vector3> { downPosition, leftPosition, rightPosition };
          
            for (int i = 0; i < potentialPositions.Count; i++)
            {
                int randomIndex = Random.Range(i, potentialPositions.Count);
                Vector3 temp = potentialPositions[i];
                potentialPositions[i] = potentialPositions[randomIndex];
                potentialPositions[randomIndex] = temp;
            }
           
            int layerMask = LayerMask.GetMask("Obstacle");
            float checkRadius = 0.5f;
            
            foreach (Vector3 position in potentialPositions)
            {              
                if (!Physics2D.OverlapCircle(position, checkRadius, layerMask))
                {                   
                    Instantiate(currentToken, position, Quaternion.identity);                
                    return;
                }
            }          
            Debug.LogError("All spawn positions are occupied.");
        }
        else
        {
            Debug.LogError("Token prefab not assigned in TopCollider script.");
        }
    }

    // Spawns Token after a redirect one is picked up
    public void SpawnTokenRedirect()
    {
        currentToken = RandomToken();

        if (currentToken != null)
        {        
            Vector3 spawnPosition = transform.position;        
            Vector3 topPosition = spawnPosition + Vector3.up * 2.5f;
            Vector3 leftPosition = spawnPosition + Vector3.left + Vector3.up * 1.5f;
            Vector3 rightPosition = spawnPosition + Vector3.right + Vector3.up * 1.5f;       
            List<Vector3> potentialPositions = new List<Vector3> { topPosition, leftPosition, rightPosition };
          
            for (int i = 0; i < potentialPositions.Count; i++)
            {
                int randomIndex = Random.Range(i, potentialPositions.Count);
                Vector3 temp = potentialPositions[i];
                potentialPositions[i] = potentialPositions[randomIndex];
                potentialPositions[randomIndex] = temp;
            }
          
            int layerMask = LayerMask.GetMask("Obstacle");         
            float checkRadius = 0.5f;         
            foreach (Vector3 position in potentialPositions)
            {              
                if (!Physics2D.OverlapCircle(position, checkRadius, layerMask))
                {                    
                    Instantiate(currentToken, position, Quaternion.identity);
                    return;
                }
            }        
            Debug.LogError("All spawn positions are occupied (Bottom Collider).");
        }
        else
        {
            Debug.LogError("Token prefab not assigned in TopCollider script.");
        }
    }

    // Spawns a random token in the appropriate position
    public GameObject RandomToken()
    {
        GameObject[] tokens = { tokenPrefab, redirectTokenPrefab, holdTokenPrefab, redTokenPrefab };
        if (rotate.pastNinety)
        {
            return tokens[Random.Range(1, tokens.Length)];
        }
        else if (rotate.pastSixty)
        {
            return tokens[Random.Range(0, tokens.Length - 1)];
        }
        else if (rotate.pastThirty)
        {
            return tokens[Random.Range(0, tokens.Length - 2)];
        }
        else
        {
            return tokenPrefab;
        }
    }
}
