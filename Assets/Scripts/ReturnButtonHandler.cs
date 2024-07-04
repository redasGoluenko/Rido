using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnButtonHandler : MonoBehaviour
{
    public Death death;
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
        Debug.Log("Button clicked");
        death.CloseMenu();
        StartCoroutine(LoadScene(1f));
    }
    IEnumerator LoadScene(float delay)
    {
       yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
