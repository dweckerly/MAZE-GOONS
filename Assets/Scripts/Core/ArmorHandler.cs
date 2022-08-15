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
    public BodyPartMapReference[] bodyPartMap;
    Dictionary<BodyMapping, ArmorObject> equipLookup = new Dictionary<BodyMapping, ArmorObject>();
    int LayerPlayer;
    
    private void Start() 
    {
        LayerPlayer = LayerMask.NameToLayer("Player");
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
                UnEquipArmor(armorBodyMap);
                continue;    
            }
            foreach (BodyPartMapReference bpmr in bodyPartMap)
            {
                if (bpmr.bodyPositionReference == armorBodyMap.bodyPositionReference)
                {
                    UnEquipArmor(armorBodyMap);
                    GameObject go = Instantiate(armorBodyMap.armorPrefab, bpmr.bodyPart.transform);
                    go.layer = LayerPlayer;
                    equipLookup[armorBodyMap.bodyPositionReference] = new ArmorObject(armor.Id, go);
                }
            }
        }
    }

    public void UnEquipArmor(ArmorBodyMap armorBodyMap)
    {
        if (equipLookup[armorBodyMap.bodyPositionReference] == null) return;
        Destroy(equipLookup[armorBodyMap.bodyPositionReference].equip);
        equipLookup[armorBodyMap.bodyPositionReference] = null;
    }
}
