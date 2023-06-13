using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyPartMapReference
{
    public GameObject bodyPart;
    public BodyMapping bodyPositionReference;
}

public class ArmorObject
{
    public string Id;
    public GameObject equip;

    public ArmorObject(string _id, GameObject _equip)
    {
        Id = _id;
        equip = _equip;
    }
}

public class ArmorHandler : MonoBehaviour
{
    public event OnEquip EquipArmorEvent;
    public event OnUnEquip UnEquipArmorEvent;
    public delegate void OnEquip(Armor Armor);
    public delegate void OnUnEquip(Armor Armor);
    private Dictionary<ArmorSlot, Armor> equippedArmor = new Dictionary<ArmorSlot, Armor>();
    public BodyPartMapReference[] bodyPartMap;
    Dictionary<BodyMapping, ArmorObject> equipLookup = new Dictionary<BodyMapping, ArmorObject>();
    int LayerInt;
    
    private void Awake() 
    {
        LayerInt = LayerMask.NameToLayer(gameObject.tag);
        foreach (BodyPartMapReference bpmr in bodyPartMap)
        {
            equipLookup.Add(bpmr.bodyPositionReference, null);
        }
    }

    public void CheckArmor(Armor armor)
    {
        if (equipLookup[armor.ArmorObjects[0].bodyPositionReference] != null && equipLookup[armor.ArmorObjects[0].bodyPositionReference].Id == armor.Id)
        {
            foreach (ArmorBodyMap armorBodyMap in armor.ArmorObjects)
            {
                UnEquipArmor(armorBodyMap, armor);
            }
            armor.equipped = false;
            UnEquipArmorEvent?.Invoke(armor);
        }
        else
        {
            if (equipLookup[armor.ArmorObjects[0].bodyPositionReference] != null)
            {
                ArmorSlot slot = ArmorSlot.Mask;
                foreach (ArmorBodyMap armorBodyMap in armor.ArmorObjects)
                {
                    switch(armorBodyMap.bodyPositionReference)
                    {
                        case BodyMapping.Head:
                            slot = ArmorSlot.Mask;
                            break;
                        case BodyMapping.LeftShoulder:
                            slot = ArmorSlot.Pauldrons;
                            break;
                        case BodyMapping.RightShoulder:
                            slot = ArmorSlot.Pauldrons;
                            break;
                        case BodyMapping.LeftWrist:
                            slot = ArmorSlot.Wrists;
                            break;
                        case BodyMapping.RightWrist:
                            slot = ArmorSlot.Wrists;
                            break;
                        case BodyMapping.Waist:
                            slot = ArmorSlot.Waist;
                            break;
                    }
                    if (equippedArmor[slot] != null) UnEquipArmor(armorBodyMap, equippedArmor[slot]);
                }
            }
            EquipArmor(armor);
            armor.equipped = true;
            EquipArmorEvent?.Invoke(armor);
        }
    }

    public void EquipArmor(Armor armor)
    {
        foreach(ArmorBodyMap armorBodyMap in armor.ArmorObjects)
        {
            foreach (BodyPartMapReference bpmr in bodyPartMap)
            {
                if (bpmr.bodyPositionReference == armorBodyMap.bodyPositionReference)
                {
                    GameObject go = Instantiate(armorBodyMap.armorPrefab, bpmr.bodyPart.transform);
                    go.layer = LayerInt;
                    foreach (Transform t in go.GetComponentInChildren<Transform>())
                    {
                        t.gameObject.layer = LayerInt;
                    }
                    equipLookup[armorBodyMap.bodyPositionReference] = new ArmorObject(armor.Id, go);
                }
            }
        }
        equippedArmor.Add(armor.slot, armor);
    }

    public void UnEquipArmor(ArmorBodyMap armorBodyMap, Armor armor)
    {
        if (equipLookup[armorBodyMap.bodyPositionReference] == null) return;
        armor.equipped = false;
        equippedArmor.Remove(armor.slot);
        UnEquipArmorEvent?.Invoke(armor);
        Destroy(equipLookup[armorBodyMap.bodyPositionReference].equip);
        equipLookup[armorBodyMap.bodyPositionReference] = null;
    }

    public void UnEquipAllArmor()
    {
        List<Armor> tempArmor = new List<Armor>();
        foreach (Armor armor in equippedArmor.Values)
        {
            tempArmor.Add(armor);
        }
        equippedArmor.Clear();

        foreach (Armor armor in tempArmor)
        {
            UnEquipArmorEvent?.Invoke(armor);
        }

        foreach (BodyMapping bodyMap in equipLookup.Keys)
        {
            if (equipLookup[bodyMap]?.equip != null) Destroy(equipLookup[bodyMap].equip);
        }
    }

    public int CalculateArmorValue()
    {
        int armorSum = 0;
        foreach(Armor armor in equippedArmor.Values)
        {
            armorSum += armor.DamageReduction;
        }
        return armorSum;
    }
}
