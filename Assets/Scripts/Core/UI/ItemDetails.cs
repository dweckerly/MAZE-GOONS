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
        itemName.text = item.name;
        description.text = item.description;
        if (item is Equippable)
        {
            if(((Equippable)item).equipped) actionBtnText.text = "Unequip";
            else actionBtnText.text = "Equip";
        }
        else
        {
            actionBtnText.text = "Use";
        }
    }
}
