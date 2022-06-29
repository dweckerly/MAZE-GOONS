using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Wall,
    Floor,
    Maze
}

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public int scale = 6;

    public TileType[,] map;

    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1, 0),
        new MapLocation(0, 1),
        new MapLocation(-1, 0),
        new MapLocation(0, -1)
    };

    MazePieceCollection mazePieceCollection;

    public virtual void Generate() { }

    private void Awake() 
    {
        mazePieceCollection = GetComponent<MazePieceCollection>();
    }

    void Start()
    {
        InitializeMap();
        Generate();
        DrawMap();
    }

    private void InitializeMap()
    {
        map = new TileType[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = TileType.Wall;
            }
        }
    }

    private void DrawMap()
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(scale, scale, scale);
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3((x - (width / 2)) * scale, 0, (z - (depth / 2)) * scale);
                if (map[x, z] == TileType.Floor)
                {   
                    MazePieceDetail piece = GetStructureAtPosition(x, z);
                    if(piece.structureType != StructureType.Undefined)
                    {
                        GameObject go = Instantiate(mazePieceCollection.pieceMap[piece.structureType], pos, Quaternion.identity);
                        go.transform.Rotate(piece.rotation);
                    }                    
                }                
            }
        }
    }

    MazePieceDetail GetStructureAtPosition(int x, int z)
    {
        TileType[] tileArray = GetSquareNeighborsTileTypes(x, z);
        foreach(MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if(tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        return new MazePieceDetail();
    }

    public int CountSquareNeighbors(int x, int z, TileType tileType)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == tileType) count++;
        if (map[x + 1, z] == tileType) count++;
        if (map[x, z - 1] == tileType) count++;
        if (map[x, z + 1] == tileType) count++;
        return count;
    }

    public int CountDiagonalNeighbors(int x, int z, TileType tileType)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == tileType) count++;
        if (map[x + 1, z - 1] == tileType) count++;
        if (map[x - 1, z + 1] == tileType) count++;
        if (map[x + 1, z + 1] == tileType) count++;
        return count;
    }

    public int CountAllFloorNeighbors(int x, int z)
    {
        return CountSquareNeighbors(x, z, TileType.Floor) + CountDiagonalNeighbors(x, z, TileType.Floor);
    }

    public TileType[] GetSquareNeighborsTileTypes(int x, int z)
    {
        return new TileType[4] { map[x - 1, z], map[x, z + 1], map[x + 1, z], map[x, z - 1] };
    }
}
