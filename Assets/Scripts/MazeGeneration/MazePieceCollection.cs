using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MazePieceDetail
{
    public string name;
    public GameObject structure;
    public TileType[] tilePattern; // tile pattern --> left, top, right, bottom
    public Vector3 rotation;
}


public class MazePieceCollection : MonoBehaviour
{   
    public List<MazePieceDetail> pieceDetails = new List<MazePieceDetail>();
}
