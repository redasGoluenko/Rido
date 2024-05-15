using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private int count = 0;
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro reference not set!");
        }
    }
    private void Update()
    {
        // Update the text with the current score
        textMeshPro.text = $"Score: {count}";
    }
    public void IncrementScore(int amount)
    {
        count+=amount;
    }
}
