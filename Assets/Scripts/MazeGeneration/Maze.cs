using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

public class MazeStructureItem
{
    public MapLocation position;
    public MazePieceDetail pieceDetail;

    public MazeStructureItem(int x, int z, MazePieceDetail _pieceDetail)
    {
        position = new MapLocation(x, z);
        pieceDetail = _pieceDetail;
    }
}

public class Maze : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public int scale = 6;

    public int minRooms = 6;
    public int maxRooms = 12;
    public int minRoomSize = 4;
    public int maxRoomSize = 20;

    public TileType[,] map;

    List<MazeStructureItem> mazeStructureMap = new List<MazeStructureItem>();

    List<List<MapLocation>> roomList = new List<List<MapLocation>>();

    NavMeshSurface surface;

    public GameObject enemyPrefab;

    public GameObject sconce;

    public GameObject chest;

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
        surface = GetComponent<NavMeshSurface>();
    }

    void Start()
    {
        InitializeMap();
        Generate();
        AddRooms(minRooms, maxRooms, minRoomSize, maxRoomSize);
        DrawMap();
        PlaceObjects();
        surface.BuildNavMesh();
        AddEnemiesToRooms();
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
            roomList.Add(new List<MapLocation>());
            int startX = Random.Range(2, width - 1);
            int startZ = Random.Range(2, depth - 1);
            int roomWidth = Random.Range(minSize, maxSize + 1);
            int roomDepth = Random.Range(minSize, maxSize + 1);
            for(int x = startX; x < width - 1 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < depth - 1 && z < startZ + roomDepth; z++)
                {
                    roomList[i].Add(new MapLocation(x, z));
                    map[x, z] = TileType.Floor;
                } 
            }
        }
    }

    private void DrawMap()
    {
        //GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //wall.transform.localScale = new Vector3(scale, scale, scale);
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = ConvertToGameSpace(x, z);
                if (map[x, z] == TileType.Floor)
                {            
                    MazePieceDetail piece = GetStructureAtPositionBySquareNeighbors(x, z);
                    GameObject go = Instantiate(mazePieceCollection.pieceMap[piece.structureType], pos, Quaternion.identity);
                    go.transform.Rotate(piece.rotation);
                    mazeStructureMap.Add(new MazeStructureItem(x, z, piece));
                }                
            }
        }
    }

    private void AddEnemiesToRooms()
    {
        int randomRoom = Random.Range(1, roomList.Count);
        roomList[randomRoom].Shuffle();
        for(int i = 0; i < 3; i++)
        {
            Vector3 pos = ConvertToGameSpace(roomList[randomRoom][i].x, roomList[randomRoom][i].z);
            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    private void PlaceObjects()
    {
        foreach(MazeStructureItem item in mazeStructureMap)
        {
            if(item.pieceDetail.structureType == StructureType.Corridor && Random.Range(0, 100) < 80)
            {
                PlaceLights(item);
            }
            else if (item.pieceDetail.structureType == StructureType.Deadend && Random.Range(0, 100) < 25)
            {
                PlaceChest(item);
            }
        }
    }

    private void PlaceLights(MazeStructureItem item)
    {
        Vector3 pos = ConvertToGameSpace(item.position.x, item.position.z);
        int chance = Random.Range(0, 100);
        if (chance > 34)
        {
            Vector3 pos1 = pos + new Vector3(0, -2, 0);
            Vector3 rotation1 = item.pieceDetail.rotation + new Vector3(0, 90, 0);
            pos1 += CalculateLightPosition(rotation1);
            GameObject go1 = Instantiate(sconce, pos1, Quaternion.identity);
            go1.transform.Rotate(rotation1);
        }
        if (chance < 65)
        {
            Vector3 pos2 = pos + new Vector3(0, -2, 0);
            Vector3 rotation2 = item.pieceDetail.rotation + new Vector3(0, -90, 0);
            pos2 += CalculateLightPosition(rotation2);
            GameObject go2 = Instantiate(sconce, pos2, Quaternion.identity);
            go2.transform.Rotate(rotation2);
        }
    }

    private void PlaceChest(MazeStructureItem item)
    {
        Vector3 pos = ConvertToGameSpace(item.position.x, item.position.z);
        pos += new Vector3(0, -3, 0);
        GameObject go = Instantiate(chest, pos, Quaternion.identity);
        Vector3 rotation = item.pieceDetail.rotation + new Vector3(0, 90, 0);
        go.transform.Rotate(rotation);
        if (Random.Range(0, 100) < 75)
        {
            Vector3 lightRotation = item.pieceDetail.rotation;
            pos += CalculateLightPosition(lightRotation);
            GameObject go2 = Instantiate(sconce, pos, Quaternion.identity);
            go2.transform.Rotate(lightRotation);
        }
    }

    private Vector3 CalculateLightPosition(Vector3 rotation)
    {
        if (rotation.y == 0) return new Vector3(0, 0, -(scale / 2));
        else if (rotation.y == 90) return new Vector3(-(scale / 2), 0, 0);
        else if (rotation.y == -90) return new Vector3((scale / 2), 0, 0);
        else return new Vector3(0, 0, (scale / 2)); // -180
    }

    Vector3 ConvertToGameSpace(int x, int z)
    {
        return new Vector3((x - (width / 2)) * scale, 0, (z - (depth / 2)) * scale);
    }

    MazePieceDetail GetStructureAtPositionBySquareNeighbors(int x, int z)
    {
        TileType[] tileArray = GetSquareNeighborsTileTypes(x, z);
        foreach (MazePieceDetail piece in mazePieceCollection.pieceDetails)
        {
            if (tileArray.IsSameArray(piece.tilePattern)) return piece;
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
