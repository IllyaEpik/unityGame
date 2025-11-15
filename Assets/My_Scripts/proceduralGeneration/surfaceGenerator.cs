using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class surfaceGenerator : MonoBehaviour
{
    public int levelWidth = 250;
    public float levelHeightChange = 15f;
    public int levelHeight = 50;
    public float noiseScale = 0.1f;

    public int enemyBotCount = 2;
    public int enemyPlantCount = 2;

    public int seed;
    private int[] surface;
    public int[][] map;

    [SerializeField] private GameObject enemyBotPrefab;
    [SerializeField] private GameObject enemyPlantPrefab;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] tilePalette;
    [Header("Параметры структур")]
    [SerializeField] private List<StructureData> structures = new List<StructureData>();
    void Start()
    {
        // LoadSeed(); // Загружаем сид, если есть
        seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);
        // Random.InitState(seed);
        PlayerPrefs.SetInt("worldSeed", seed);
        generateSurface();
    }

    public void LoadSeed(int loadedSeed)
    {
        seed = loadedSeed;
        Random.InitState(seed);
        generateSurface();
        // if (PlayerPrefs.HasKey("worldSeed"))
        // {
        //     seed = PlayerPrefs.GetInt("worldSeed");
        //     Debug.Log($"Loaded seed: {seed}");
        // }
        // else
        // {
        //     seed = System.DateTime.Now.Millisecond;
        //     PlayerPrefs.SetInt("worldSeed", seed);
        //     PlayerPrefs.Save();
        //     Debug.Log($"Generated new seed: {seed}");
        // }

        
        
    }

    public void generateSurface()
    {
        surface = new int[levelWidth * 2];
        map = new int[levelWidth * 2][];
        groundTilemap.ClearAllTiles();
        float count = 0f;
        float offset = Random.Range(0f, 9999f);
        int structureCoors = Random.Range(-levelWidth+2, levelWidth-2);
        
        // Генерация в обе стороны от центра (влево и вправо)
        for (int i = -levelWidth; i < levelWidth; i++)
        {
            if (count <= 0)
            {
                int xIndex = i + levelWidth; 
                map[xIndex] = new int[levelHeight];
                float floatY = Mathf.PerlinNoise((i + offset) * noiseScale, 0) * levelHeightChange;
                int y = Mathf.FloorToInt(floatY);
                surface[xIndex] = y;
                if (structureCoors==i)
                {
                    Debug.Log(structureCoors);
                    Debug.Log(i);
                    count = structures[0].prefabWidth;
                    Instantiate(structures[0].prefab, 
                    new Vector2(
                        (structureCoors+2.5f+structures[0].prefabWidth/2)*5, 
                        (-y-structures[0].prefabWidth/2)*5-0.5f ), 
                        Quaternion.identity);
                }
                for (int countY = 0; countY < levelHeight; countY++)
                {
                    if (countY > y)
                    {
                        map[xIndex][countY] = 1;
                        TileBase tileToPlace = tilePalette[0];
                        groundTilemap.SetTile(new Vector3Int(i, -countY, 0), tileToPlace);
                    }
                }
            }
            else
            {
                count--;
            }
            
        }

        // generateStructure(npcPrefab);
        // generateEnemies(enemyBotCount, enemyBotPrefab);
        // generateEnemies(enemyPlantCount, enemyPlantPrefab);
    }

    void generateEnemies(int count, GameObject prefab)
    {
        for (int e = 0; e < count; e++)
        {
            float x = Random.Range(-levelWidth, levelWidth);
            Instantiate(prefab, new Vector2(x, 0), Quaternion.identity);
        }
    }

    void generateStructure(GameObject prefab)
    {
        // int middleIndex = surface.Length / 2;
        int currentYLeft = surface[0]+1;
        int currentYRight = surface[surface.Length-1]+1;

        TileBase tileToPlace = tilePalette[1];
        int distance = 25;

        // Генерация платформы справа
        for (int x = 0; x < distance; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(levelWidth + x, -currentYRight - y, 0), tileToPlace);
            }
        }

        // Генерация платформы слева
        for (int x = 0; x < distance; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(-levelWidth - x, -currentYLeft - y, 0), tileToPlace);
            }
        }

        // Спавн NPC на обеих сторонах
        Instantiate(prefab, new Vector2((levelWidth+distance/2 + 2.5f)*5, -currentYRight-3 ), Quaternion.identity);
        Instantiate(prefab, new Vector2((-levelWidth-distance/2 - 2.5f)*5, -currentYLeft-3 ), Quaternion.identity);
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

}