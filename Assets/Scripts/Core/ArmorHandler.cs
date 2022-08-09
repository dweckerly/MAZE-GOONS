using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyPartMapReference
{
    public GameObject bodyPart;
    public BodyMapping bodyPositionReference;
}

public class ArmorHandler : MonoBehaviour
{
    public BodyPartMapReference[] bodyPartMap;
    Dictionary<BodyMapping, GameObject> equipLookup = new Dictionary<BodyMapping, GameObject>();

    public void EquipArmor(Armor armor)
    {
        foreach(ArmorBodyMap armorBodyMap in armor.ArmorObjects)
        {
            if (equipLookup[armorBodyMap.bodyPositionReference] != null)
            { 
                Destroy(equipLookup[armorBodyMap.bodyPositionReference]);
                equipLookup[armorBodyMap.bodyPositionReference] = null;
            }
            foreach(BodyPartMapReference bpmr in bodyPartMap)
            {
                if (bpmr.bodyPositionReference == armorBodyMap.bodyPositionReference)
                {
                    GameObject go = Instantiate(armorBodyMap.armorPrefab, bpmr.bodyPart.transform);
                    equipLookup[armorBodyMap.bodyPositionReference] = go;
                }
            }
        }
    }
}
