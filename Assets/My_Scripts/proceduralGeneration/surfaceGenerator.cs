using UnityEngine;
using UnityEngine.Tilemaps;

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

        float offset = Random.Range(0f, 9999f);

        // Генерация в обе стороны от центра (влево и вправо)
        for (int i = -levelWidth; i < levelWidth; i++)
        {
            int xIndex = i + levelWidth;
            map[xIndex] = new int[levelHeight];
            float floatY = Mathf.PerlinNoise((i + offset) * noiseScale, 0) * levelHeightChange;
            int y = Mathf.FloorToInt(floatY);
            surface[xIndex] = y;

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

        generateStructure(npcPrefab);
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
}