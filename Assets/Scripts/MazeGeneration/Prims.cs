using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims : Maze
{
    List<MapLocation> walls = new List<MapLocation>();

    public override void Generate()
    {
        int x = Random.Range(1, width - 1);
        int z = Random.Range(1, depth - 1);
        map[x, z] = TileType.Floor;
        
        AddWallsToList(x, z);

        while(walls.Count > 0)
        {
            int randomWall = Random.Range(0, walls.Count);
            x = walls[randomWall].x;
            z = walls[randomWall].z;
            walls.RemoveAt(randomWall);
            if(CountSquareNeighbors(x, z, TileType.Floor) == 1)
            {
                map[x, z] = TileType.Floor;
                AddWallsToList(x, z);
            }
        }
    }

    void AddWallsToList(int x, int z)
    {
        walls.Add(new MapLocation(x - 1, z));
        walls.Add(new MapLocation(x + 1, z));
        walls.Add(new MapLocation(x, z - 1));
        walls.Add(new MapLocation(x, z + 1));
    }
}
