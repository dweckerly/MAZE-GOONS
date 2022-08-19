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
    public int damageReduction = 0;

    public ArmorObject(string _id, GameObject _equip, int _damageReduction)
    {
        Id = _id;
        equip = _equip;
        damageReduction = _damageReduction;
    }
}

public class ArmorHandler : MonoBehaviour
{
    public event OnEquip EquipArmorEvent;
    public event OnUnEquip UnEquipArmorEvent;
    public delegate void OnEquip(Armor Armor);
    public delegate void OnUnEquip(Armor Armor);
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

    public void EquipArmor(Armor armor)
    {
        foreach(ArmorBodyMap armorBodyMap in armor.ArmorObjects)
        {
            if (equipLookup[armorBodyMap.bodyPositionReference] != null && equipLookup[armorBodyMap.bodyPositionReference].Id == armor.Id)
            {
                UnEquipArmor(armorBodyMap, armor);
            }
            else 
            {
                foreach (BodyPartMapReference bpmr in bodyPartMap)
                {
                    if (bpmr.bodyPositionReference == armorBodyMap.bodyPositionReference)
                    {
                        UnEquipArmor(armorBodyMap, armor);
                        GameObject go = Instantiate(armorBodyMap.armorPrefab, bpmr.bodyPart.transform);
                        go.layer = LayerInt;
                        equipLookup[armorBodyMap.bodyPositionReference] = new ArmorObject(armor.Id, go, armor.DamageReduction);
                    }
                }
                EquipArmorEvent?.Invoke(armor);
            }
            
        }
    }

    public int CalculateArmorValue()
    {
        int armorSum = 0;
        foreach(KeyValuePair<BodyMapping, ArmorObject> equip in equipLookup)
        {
            if (equip.Value != null) armorSum += equip.Value.damageReduction;
        }
        return armorSum;
    }

    public void UnEquipArmor(ArmorBodyMap armorBodyMap, Armor armor)
    {
        if (equipLookup[armorBodyMap.bodyPositionReference] == null) return;
        UnEquipArmorEvent?.Invoke(armor);
        Destroy(equipLookup[armorBodyMap.bodyPositionReference].equip);
        equipLookup[armorBodyMap.bodyPositionReference] = null;
    }
}
