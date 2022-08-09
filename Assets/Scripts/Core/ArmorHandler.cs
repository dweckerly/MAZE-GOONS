using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorHandler : MonoBehaviour
{
    public BodyPartMap bodyPartMap;
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
            foreach(BodyPartMapReference bpmr in bodyPartMap.bodyMap)
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
