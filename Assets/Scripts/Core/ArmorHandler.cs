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
            UnEquipArmorEvent?.Invoke(armor);
        }
        else
        {
            if (equipLookup[armor.ArmorObjects[0].bodyPositionReference] != null)
            {
                foreach (ArmorBodyMap armorBodyMap in armor.ArmorObjects)
                {
                    UnEquipArmor(armorBodyMap, armor);
                }
                UnEquipArmorEvent?.Invoke(armor);
            }
            EquipArmor(armor);
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
        equippedArmor.Remove(armor.slot);
        UnEquipArmorEvent?.Invoke(armor);
        Destroy(equipLookup[armorBodyMap.bodyPositionReference].equip);
        equipLookup[armorBodyMap.bodyPositionReference] = null;
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
