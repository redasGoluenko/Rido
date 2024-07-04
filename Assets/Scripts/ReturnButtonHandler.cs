using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButtonHandler : MonoBehaviour
{
    public Color newColor; // Color to change backgrounds to
    private float fadeDuration = 0.125f; // Duration of the fade in seconds

    public MoveDiagonally endless; // Reference to the MoveDiagonally script
    public MoveDiagonally levels; // Reference to the MoveDiagonally script
    public MoveDiagonally store; // Reference to the MoveDiagonally script
    public MoveDiagonally exit; // Reference to the MoveDiagonally script
    public Death death; // Reference to the Death script
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void OnButtonClick()
    {   
        // Start the coroutine to move exit diagonally left
        StartCoroutine(exit.MoveLeftDiagonallyInitiallyCoroutine());
        StartCoroutine(WaitAndMove(0.1f, store)); // Wait 1 second, then move store diagonally left
        StartCoroutine(WaitAndMove(0.2f, levels)); // Wait 2 seconds, then move levels diagonally left
        StartCoroutine(WaitAndMove(0.3f, endless)); // Wait 3 seconds, then move endless diagonally left       
        death.CloseMenu(); // Close the menu       
        StartCoroutine(LoadScene(1f)); // Wait 4 seconds, then load the scene "Endless"
    }  
    // Coroutine to wait for a specified time and then move the object diagonally left
    IEnumerator WaitAndMove(float delay, MoveDiagonally moveDiagonally)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        StartCoroutine(moveDiagonally.MoveLeftDiagonallyInitiallyCoroutine()); // Start moving the object diagonally left
    }

    // Coroutine to wait for a specified time and then load the scene "Endless"
    IEnumerator LoadScene(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"); // Load the scene "Endless"
    } 
}
