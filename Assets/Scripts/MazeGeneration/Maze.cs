using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Wall,
    Floor,
    Maze
}

public enum Direction
{
    West,
    North,
    East, 
    South
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
        AddRooms(4, 10, 4, 10);
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

    public virtual void AddRooms(int minRooms, int maxRooms, int minSize, int maxSize)
    {
        int roomCount = Random.Range(minRooms, maxRooms + 1);
        for (int i = 0; i < roomCount; i++)
        {
            int startX = Random.Range(2, width - 1);
            int startZ = Random.Range(2, depth - 1);
            int roomWidth = Random.Range(minSize, maxSize + 1);
            int roomDepth = Random.Range(minSize, maxSize + 1);
            for(int x = startX; x < width - 1 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < depth - 1 && z < startZ + roomDepth; z++)
                {
                    map[x, z] = TileType.Floor;
                } 
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
                    // check for floor (check all neighbors)
                    // check for wall (check for 5 floor in direction and wall)
                    // check for corridor (check 4 squares)
                    MazePieceDetail piece = GetStructureAtPositionByAllNeighbors(x, z);
                    if(piece.structureType != StructureType.Undefined)
                    {
                        GameObject go = Instantiate(mazePieceCollection.pieceMap[piece.structureType], pos, Quaternion.identity);
                        go.transform.Rotate(piece.rotation);
                    }
                    else 
                    {
                        piece = GetStructureAtPositionByDirectionalNeighbors(x, z);
                        if (piece.structureType != StructureType.Undefined)
                        {
                            GameObject go = Instantiate(mazePieceCollection.pieceMap[piece.structureType], pos, Quaternion.identity);
                            go.transform.Rotate(piece.rotation);
                        }
                        else 
                        {
                            piece = GetStructureAtPositionBySquareNeighbors(x, z);
                            if (piece.structureType != StructureType.Undefined)
                            {
                                GameObject go = Instantiate(mazePieceCollection.pieceMap[piece.structureType], pos, Quaternion.identity);
                                go.transform.Rotate(piece.rotation);
                            }
                            else
                            {
                                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                block.transform.localScale = new Vector3(scale, scale, scale);
                                block.transform.position = pos;
                            }
                        }
                                               
                    }                    
                }                
            }
        }
    }

    MazePieceDetail GetStructureAtPositionByAllNeighbors(int x, int z)
    {
        TileType[] tileArray = GetNeighborTileTypes(x, z);
        foreach(MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if(tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        return new MazePieceDetail();
    }

    MazePieceDetail GetStructureAtPositionByDirectionalNeighbors(int x, int z)
    {
        TileType[] tileArray = GetDirectionalTileTypes(x, z, Direction.West);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        tileArray = GetDirectionalTileTypes(x, z, Direction.North);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        tileArray = GetDirectionalTileTypes(x, z, Direction.East);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        tileArray = GetDirectionalTileTypes(x, z, Direction.South);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        return new MazePieceDetail();
    }

    MazePieceDetail GetStructureAtPositionBySquareNeighbors(int x, int z)
    {
        TileType[] tileArray = GetSquareNeighborsTileTypes(x, z);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.CompareArrays(piece.tilePattern)) return piece;
        }
        return new MazePieceDetail();
    }

    public int CountSquareNeighborsByType(int x, int z, TileType tileType)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == tileType) count++;
        if (map[x + 1, z] == tileType) count++;
        if (map[x, z - 1] == tileType) count++;
        if (map[x, z + 1] == tileType) count++;
        return count;
    }

    public int CountDiagonalNeighborsByType(int x, int z, TileType tileType)
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
        return CountSquareNeighborsByType(x, z, TileType.Floor) + CountDiagonalNeighborsByType(x, z, TileType.Floor);
    }

    public TileType[] GetNeighborTileTypes(int x, int z)
    {
        // left, top left, top middle, top right, right, bottom right, bottom mid, bottom left
        return new TileType[8] {
            map[x - 1, z],
            map[x - 1, z + 1], 
            map[x, z + 1],
            map[x + 1, z + 1],
            map[x + 1, z],
            map[x + 1, z - 1],
            map[x, z - 1],
            map[x - 1, z - 1]
        };
    }

    public TileType[] GetDirectionalTileTypes(int x, int z, Direction dir)
    {
        // start at left including where 5 floor tiles should be and 1 wall
        if (dir == Direction.West)
            return new TileType[6] { map[x - 1, z], map[x - 1, z + 1], map[x, z + 1], map[x + 1, z], map[x, z - 1], map[x - 1, z - 1] };
        else if (dir == Direction.North)
            return new TileType[6] { map[x - 1, z], map[x - 1, z + 1], map[x, z + 1], map[x + 1, z + 1], map[x + 1, z], map[x, z - 1] };
        else if (dir == Direction.East)
            return new TileType[6] { map[x - 1, z], map[x, z + 1], map[x + 1, z + 1], map[x + 1, z], map[x + 1, z - 1], map[x, z - 1] };
        else if (dir == Direction.South)
            return new TileType[6] { map[x - 1, z], map[x, z + 1], map[x + 1, z], map[x + 1, z - 1], map[x, z - 1], map[x - 1, z - 1] };
        return new TileType[0];
    }

    public TileType[] GetSquareNeighborsTileTypes(int x, int z)
    {
        // tile pattern --> left (x - 1), top (z + 1), right (x + 1), bottom (z - 1)
        return new TileType[4] { map[x - 1, z], map[x, z + 1], map[x + 1, z], map[x, z - 1] };
    }

    public TileType[] GetDiagonalNeighborsTileTypes(int x, int z)
    {
        // top left, top right, bottom right, bottom left
        return new TileType[4] { map[x - 1, z + 1], map[x + 1, z + 1], map[x + 1, z - 1], map[x - 1, z - 1] };
    }
}
