using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyMapping
{
    Head,
    LeftShoulder,
    RightShoulder,
    LeftWrist,
    RightWrist
}

[System.Serializable]
public class ArmorBodyMap
{
    public GameObject armorPrefab;
    public BodyMapping bodyPositionReference;
}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/New Armor", order = 1)]
public class Armor : Item
{
    public ArmorBodyMap[] ArmorObjects;
}
