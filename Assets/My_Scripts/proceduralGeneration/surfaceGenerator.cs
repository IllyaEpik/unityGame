using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class surfaceGenerator : MonoBehaviour
{
    [Header("Основные параметры мира")]
    public int levelWidth = 250;
    public float levelHeightChange = 15f;
    public int levelHeight = 50;
    public float noiseScale = 0.1f;

    [Header("Параметры врагов")]
    public int enemyBotCount = 2;
    public int enemyPlantCount = 2;

    [Header("Параметры структур")]
    [SerializeField] private List<StructureData> structures = new List<StructureData>();

    [Header("Прочее")]
    public int seed; // публичный сид
    private int[] surface;
    public int[][] map;

    [SerializeField] private GameObject enemyBotPrefab;
    [SerializeField] private GameObject enemyPlantPrefab;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] tilePalette;

    private const int tileSize = 10; // размер тайла и структуры в юнитах

    void Start()
    {
        if (seed == 0)
            seed = System.DateTime.Now.Millisecond;

        Random.InitState(seed);
        PlayerPrefs.SetInt("worldSeed", seed);
        generateSurface();
    }

    public void LoadSeed(int loadedSeed)
    {
        seed = loadedSeed;
        Random.InitState(seed);
        generateSurface();
    }

    public void generateSurface()
    {
        surface = new int[levelWidth * 2];
        map = new int[levelWidth * 2][];
        groundTilemap.ClearAllTiles();

        float offset = Random.Range(0f, 9999f);
        int count = 0;
        int i = -levelWidth;
        while (i < levelWidth)
        {
            int xIndex = i + levelWidth;
            map[xIndex] = new int[levelHeight];

            // Генерация поверхности
            float floatY = Mathf.PerlinNoise((i + offset) * noiseScale, 0) * levelHeightChange;
            int y = Mathf.FloorToInt(floatY);
            surface[xIndex] = y;

            bool spawnedStructure = false;

            // Проверяем все структуры
            for (int s = 0; s < structures.Count; s++)
            {
                var structure = structures[s];

                if (structure.amount > 0)
                {
                    // Спавн структуры по X, ближе к центру
                    float worldX = i * tileSize / 3f;
                    Vector3 worldPos = new Vector3(worldX, -surface[xIndex] * tileSize, 0);
                    count = structure.widthInTiles;
                    Instantiate(structure.prefab, worldPos, Quaternion.identity);

                    structure.amount--;
                    spawnedStructure = true;

                    // Пропускаем тайлы под структурой
                    // i += structure.widthInTiles;
                    break;
                }
            }

            if (!spawnedStructure && count <= 0)
            {
                // Генерация тайлов земли только если структура не спавнится
                for (int countY = 0; countY < levelHeight; countY++)
                {
                    if (countY > surface[xIndex])
                    {
                        map[xIndex][countY] = 1;
                        Vector3Int tilePos = new Vector3Int(i, -countY, 0);
                        groundTilemap.SetTile(tilePos, tilePalette[0]);
                    }
                }
                i++; // идем на следующий тайл
            }
            else
            {
                count--;
                i++;
            }
        }
            
        // Генерация врагов
        generateEnemies(enemyBotCount, enemyBotPrefab);
        generateEnemies(enemyPlantCount, enemyPlantPrefab);
    }

    void generateEnemies(int count, GameObject prefab)
    {
        for (int e = 0; e < count; e++)
        {
            int xIndex = Random.Range(0, surface.Length);
            float surfaceYTile = surface[xIndex];
            Vector3 pos = new Vector3((xIndex - levelWidth) * tileSize, -surfaceYTile * tileSize + 1, 0);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}

[System.Serializable]
public class StructureData
{
    public GameObject prefab;
    public int amount = 1;
    public float prefabWidth = 30f; // ширина префаба в юнитах
    public int widthInTiles
    {
        get
        {
            return Mathf.CeilToInt(prefabWidth / 10f); // tileSize = 10
        }
    }
}

/*
Вариант с рандомным выбором структуры:
if (Random.value < 0.05f && structure.amount > 0)
{
    float worldX = i * tileSize / 3f;
    Vector3 worldPos = new Vector3(worldX, -surface[xIndex] * tileSize, 0);
    Instantiate(structure.prefab, worldPos, Quaternion.identity);
    structure.amount--;
    i += structure.widthInTiles;
}
*/