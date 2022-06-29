using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureType
{
    Undefined,
    Straight,
    Corner,
    Crossroads,
    Deadend,
    TJunction
}

[System.Serializable]
public class StructureTypeMapping
{
    public StructureType structureType;
    public GameObject structure;
}

[System.Serializable]
public struct MazePieceDetail
{
    public string name;
    public StructureType structureType;
    public TileType[] tilePattern; // tile pattern --> left (x - 1), top (z + 1), right (x + 1), bottom (z - 1)
    public Vector3 rotation;
}

public class MazePieceCollection : MonoBehaviour
{   
    public List<StructureTypeMapping> structureMapping = new List<StructureTypeMapping>();
    public List<MazePieceDetail> pieceDetails = new List<MazePieceDetail>();
    public Dictionary<StructureType, GameObject> pieceMap = new Dictionary<StructureType, GameObject>();

    private void Awake() 
    {
        foreach(StructureTypeMapping mapping in structureMapping)
        {
            pieceMap.Add(mapping.structureType, mapping.structure);
        }
    }
}
