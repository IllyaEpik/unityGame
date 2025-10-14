using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private int platformsPerSpawn = 3;
    [SerializeField] private float minX = -1f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float maxY = 20f;

    private float lastY = 0f;

    void Start()
    {
        SpawnMultiplePlatforms();
    }

    void SpawnMultiplePlatforms()
    {
        for (int i = 0; i < platformsPerSpawn; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        float x = Random.Range(minX, maxX);
        float y = lastY + Random.Range(minY, maxY);
        Vector3 pos = new Vector3(x, y, 0f);

        int prefabIndex = Random.Range(0, platformPrefabs.Length);
        Instantiate(platformPrefabs[prefabIndex], pos, Quaternion.identity);

        lastY = y;
    }

    public void SpawnNextPlatforms()
    {
        SpawnMultiplePlatforms();
    }
}