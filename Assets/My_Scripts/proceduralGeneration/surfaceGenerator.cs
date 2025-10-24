using System.Threading;
using NUnit.Framework.Constraints;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
public class surfaceGenerator : MonoBehaviour
{
    public int levelWidth = 250;
    public float levelHeightChange = 15f;
    public int levelHeight = 50;
    public int[][] map;
    public int[][] structure;
    public int enemyBotCount = 2;
    public int enemyPlantCount = 2;

    public float noiseScale = 0.1f;
    // public GameObject groundTilePrefab;
    private System.Random random = new System.Random();
    private int[] surface;
    [SerializeField] private GameObject enemyBotPrefab;
    [SerializeField] private GameObject enemyPlantPrefab;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] tilePalette;
    public int seed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateSurface();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void generateSurface()
    {
        surface = new int[levelWidth];
        map = new int[levelWidth][];
        if (seed == 0)
            seed = System.DateTime.Now.Millisecond;

        Random.InitState(seed);
        float offset = Random.Range(0f, 9999f);


        for (int x = 0; x < levelWidth; x++)
        {
            map[x] = new int[levelHeight];
            float floatY = Mathf.PerlinNoise((x + offset) * noiseScale, 0) * levelHeightChange;
            // if (y>)
            int y = Mathf.FloorToInt(floatY);
            surface[x] = y;
            // Instantiate(groundTilePrefab, new Vector2(x, y), Quaternion.identity, transform);

            for (int countY = 0; countY < levelHeight; countY++)
            {
                if (countY > y)
                {

                    map[x][countY] = 1;
                    TileBase tileToPlace = tilePalette[0];
                    groundTilemap.SetTile(new Vector3Int(x - levelWidth / 2, -countY, 0), tileToPlace);
                }
                // else if (countY+1==y && random.Next(0,5)==1)
                // {
                //     if (enemyBotCountLeft > 0 && random.Next(0,5)==1)
                //     {
                //         enemyBotCountLeft--;
                //         
                //     }else if (enemyPlantCount > 0)
                //     {
                //         enemyPlantCountLeft--;
                //     }
                // }
            }
        }
        // generateEnemies(enemyBotCount, enemyBotPrefab);
        // generateEnemies(enemyPlantCount, enemyPlantPrefab);
        generateStructure(npcPrefab);
    }
    void generateEnemies(int count, GameObject prefab)
    {


        // float offset = Random.Range(0f, 9999f);
        for (int e = 0; e < count; e++)
        {
            // Mathf.FloorToInt()
            float x = Random.Range(0f, levelWidth * 4);
            // int y = surface[x];
            Instantiate(prefab, new Vector2(x - levelWidth * 2, 0), Quaternion.identity);
        }
    }
    // 4.9
    void generateStructure(GameObject prefab)
    {
        int currentY = surface[surface.Length - 1] + 1;
        Debug.Log(currentY);
        TileBase tileToPlace = tilePalette[0];
        int distance = 25;
        for (int x = 0; x < distance; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(levelWidth / 2 + x, -currentY-y, 0), tileToPlace);
                
            }
        }
        Instantiate(prefab, new Vector2((levelWidth / 2 +distance/2)*5 + 2.5f, currentY*-5+10), Quaternion.identity);
    }
}
