using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pick random starting point
// generate paths that move in random directions
// if path meets starting point add it to the "maze"
// generate more random paths and add them to the maze if they connect to it
// Maze keeps getting bigger and bigger as more paths are added to it

public class Wilsons : Maze
{
    List<MapLocation> availableCells = new List<MapLocation>();

    public override void Generate()
    {
        int x = Random.Range(1, width - 1);
        int z = Random.Range(1, depth - 1);
        map[x, z] = TileType.Maze;
        while(GetAvailableCells() > 1)
        {
            RandomWalk();
        }
    }

    int GetAvailableCells()
    {
        availableCells.Clear();
        for(int z = 1; z < depth - 1; z++)
        {
            for(int x = 1; x < width - 1; x++)
            {
                if (CountSquareNeighbors(x, z, TileType.Maze) == 0)
                {
                    availableCells.Add(new MapLocation(x, z));
                }
            }
        }
        return availableCells.Count;
    }

    void RandomWalk()
    {
        List<MapLocation> traversed = new List<MapLocation>();
        int randomStartIndex = Random.Range(0, availableCells.Count);
        int currentX = availableCells[randomStartIndex].x;
        int currentZ = availableCells[randomStartIndex].z;

        map[currentX, currentZ] = TileType.Floor;
        traversed.Add(new MapLocation(currentX, currentZ));

        bool validPath = false;

        int loop = 0;

        while(currentX > 0 && currentX < width - 1 && currentZ > 0 && currentZ < depth - 1 && loop < 50000 && !validPath)
        {
            // this check is supposed to prevent rooms from forming (making it a "perfect" maze) but just send the script into an endless loop
            // need to investigate further... 
            //if (CountSquareNeighbors(currentX, currentZ, TileType.Maze) > 1) return;

            int randomDirection = Random.Range(0, directions.Count);
            int newX = currentX + directions[randomDirection].x;
            int newZ = currentZ + directions[randomDirection].z;
            if (CountSquareNeighbors(newX, newZ, TileType.Floor) < 2)
            {
                currentX = newX;
                currentZ = newZ;
                map[currentX, currentZ] = TileType.Floor;
                traversed.Add(new MapLocation(currentX, currentZ));
            }
            validPath = CountSquareNeighbors(currentX, currentZ, TileType.Maze) == 1;
            
            loop++;
            if (loop >= 50000) print("LOOP COUNT TERMINATION");
        }

        if(validPath)
        {
            print("Path found!");
            foreach(MapLocation loc in traversed)
            {
                map[loc.x, loc.z] = TileType.Maze;
            }
        }
        else 
        {
            foreach (MapLocation loc in traversed)
            {
                map[loc.x, loc.z] = TileType.Wall;
            }
        }
    }
}
