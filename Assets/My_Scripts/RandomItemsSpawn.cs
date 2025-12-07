using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class RandomItemsSpawn : MonoBehaviour
{
    [System.Serializable]
    public class SpawnItems
    {
        public GameObject prefab;
        public int amount;
    }

    [Header("Список поверхностей")]
    [SerializeField] private surfaceGenerator surface;

    [Header("Y и Z для размещения объектов")]
    public float yLevel = 0f;
    public float zLevel = 0f;

    [Header("префабы предметов")]
    public List<SpawnItems> prefabs;

    private bool[] occupied;

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        int[] surfaceArr = surface.surface;
        occupied = new bool[surfaceArr.Length]; // все клетки свободны изначально

        foreach (var item in prefabs)
        {
            int spawned = 0;
            int attempts = 0;
            while (spawned < item.amount && attempts < 100)
            {
                attempts++;
                int cellIndex = Random.Range(0, surfaceArr.Length);

                if (occupied[cellIndex])
                    continue; // клетка занята, пробуем другую

                // X и Y позиции
                float xPos = cellIndex - surface.levelWidth; // смещаем центр
                float yPos = surfaceArr[cellIndex] + 1f;     // над поверхностью
                Vector3 pos = new Vector3(xPos, yPos, zLevel);

                Instantiate(item.prefab, pos, Quaternion.identity);

                occupied[cellIndex] = true; // отмечаем клетку как занятую
                spawned++;
            }

            if (spawned < item.amount)
            {
                Debug.LogWarning("Не удалось спавнить все {item.prefab.name}, возможно мало клеток.");
            }
        }
    }
}
