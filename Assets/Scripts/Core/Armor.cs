using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyMapping
{
    Head,
    LeftShoulder,
    RightShoulder,
    LeftWrist,
    RightWrist,
    Waist,
    Body,
    Gloves
}

public enum ArmorSlot
{
    Mask,
    Pauldrons,
    Wrists,
    Waist,
    Body,
    Gloves
}

[System.Serializable]
public class ArmorBodyMap
{
    public GameObject armorPrefab;
    public BodyMapping bodyPositionReference;
}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/New Armor", order = 1)]
public class Armor : Equippable
{
    public ArmorBodyMap[] ArmorObjects;
    public override ItemType itemType => ItemType.Armor;
    public ArmorSlot slot;
    public int DamageReduction = 1;
    public Material[] materials;
    public Mesh BodyMesh;
}
