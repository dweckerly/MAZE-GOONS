using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text props;
    public TMP_Text description;
    public TMP_Text actionBtnText;

    public void ShowDetails(Item item)
    {
        props.text = "";
        itemName.text = item.itemName;
        description.text = item.description;
        
        if (item is Equippable)
        {
            if(((Equippable)item).equipped) actionBtnText.text = "Unequip";
            else actionBtnText.text = "Equip";
            if (item is Weapon) props.text = "ATK - " + ((Weapon)item).weaponDamage;
            if (item is Armor) props.text = "DEF - " + ((Armor)item).DamageReduction;
            if (item is Shield) props.text = "DEF - " + ((Shield)item).DamageReduction;
        }
        else
        {
            actionBtnText.text = "Use";
        }
        props.text += "\r\nWeight - " + item.weight;
    }
}
