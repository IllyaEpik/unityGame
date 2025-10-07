using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using UnityEngine.Tilemaps;
using UnityEditor.U2D.Aseprite;

public class ProceduralGeneration : MonoBehaviour
{
    private int width = 100;
    private int height = 100;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] tilePalette;
    [SerializeField] private int[][] map = new int[100][];
    System.Random random = new System.Random();
    void Start()
    {
        GenerateLevel();
    }
    private int check(bool condition, int row, int cell)
    {
        if (condition)
        {
            return map[row][cell] != 0 ? 1 : 0;
        }
        return 0;
    }
    void Generatecaves()
    {

        int[][] Newmap = new int[100][];
        int count = 0;
        for (int row = 0; row < width; row++)
        {
            Newmap[row] = new int[height];
            for (int cell = 0; cell < height; cell++)
            {
                count = 0;
                count += check(row > 0, row - 1, cell);
                count += check(row < 99, row + 1, cell);
                count += check(cell > 0, row, cell - 1);
                count += check(cell < 99, row, cell + 1);

                count += check(row > 0 && cell > 0, row - 1, cell - 1);
                count += check(row < 99 && cell > 0, row + 1, cell - 1);
                count += check(row > 0 && cell < 99, row - 1, cell + 1);
                count += check(row < 99 && cell < 99, row + 1, cell + 1);

                if (count < 4)
                {
                    Newmap[row][cell] = 0;
                }
                else if (count > 5)
                {
                    Newmap[row][cell] = 1;
                }
                Debug.Log(count);
            }

        }
        map = Newmap;
    }
    void GenerateLevel()
    {
        for (int row = 0; row < width; row++)
        {
            map[row] = new int[height];
            for (int cell = 0; cell < height; cell++)
            {
                map[row][cell] = cell;
                if (row < 5)
                {
                    map[row][cell] = 0;
                }
                else
                {
                    map[row][cell] = random.Next(0, 5) == 1 ? 0 : 1;
                }
            }
        }
        Generatecaves();
        Generatecaves();
        Generatecaves();
        Generatecaves();
        Generatecaves();
        for (int row = 0; row < width; row++)
        {
            for (int cell = 0; cell < height; cell++)
            {
                int tileTypeIndex = map[row][cell];
                if (tileTypeIndex!=0)
                {
                    TileBase tileToPlace = tilePalette[tileTypeIndex-1];
                        
                    groundTilemap.SetTile(new Vector3Int(-cell+10, -row, 0), tileToPlace);
                }
            }
        }
        Debug.Log("work complete");

    }
}