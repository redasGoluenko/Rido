using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    // Prefab to spawn
    public GameObject prefab;
    // Initial size of the prefab
    public Vector3 initialSize = new Vector3(1f, 1f, 1f);
    // Target size to reach before destroying the prefab
    public Vector3 targetSize = new Vector3(3f, 3f, 3f);
    // Speed of the size increase
    public float growthSpeed = 0.1f;

    private GameObject currentInstance;
    private bool isGrowing;

    void Start()
    {
        SpawnPrefab();
    }

    void Update()
    {
        if (isGrowing && currentInstance != null && PlayerManager.instance.levelSelected == 1)
        {
            currentInstance.SetActive(true);
            // Increase the size of the current instance
            currentInstance.transform.localScale = Vector3.Lerp(currentInstance.transform.localScale, targetSize, growthSpeed * Time.deltaTime);

            // Check if the current instance has reached or exceeded the target size
            if (Vector3.Distance(currentInstance.transform.localScale, targetSize) < 0.01f)
            {
                Destroy(currentInstance);
                isGrowing = false;
                SpawnPrefab();
            }
        }

        if(PlayerManager.instance.levelSelected != 1)
        {
            if (currentInstance != null)
            {
                currentInstance.SetActive(false);
            }
        }
    }

    void SpawnPrefab()
    {
        // Instantiate the prefab at the position of the parent object
        currentInstance = Instantiate(prefab, transform.position, Quaternion.identity);
        // Set the initial size of the prefab
        currentInstance.transform.localScale = initialSize;
        currentInstance.SetActive(false);
        // Start the growth process
        isGrowing = true;
    }
}
