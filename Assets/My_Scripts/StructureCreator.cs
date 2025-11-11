using UnityEngine;

public class StructureCreator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Список возможных структур (префабы комнат)")]
    public GameObject[] structures;

    [Header("Сколько структур создать")]
    public int structureCount = 5;

    [Header("Размер области генерации (например, 100x100)")]
    public Vector2 generationArea = new Vector2(100, 100);

    void Start()
    {
        GenerateStructures();
    }
    void Update()
    {
        
    }

    void GenerateStructures()
    {
        for (int i = 0; i < structureCount; i++)
        {
            // Случайная позиция
            Vector2 pos = new Vector2(
                Random.Range(-generationArea.x / 2, generationArea.x / 2),
                0
                // Random.Range(-generationArea.y / 2, generationArea.y / 2)
            );

            // Случайная структура из списка
            GameObject prefab = structures[Random.Range(0, structures.Length)];

            // Создание
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
