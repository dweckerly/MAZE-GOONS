using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI TypeText;
    public TextMeshProUGUI WeightText;
    public TextMeshProUGUI AmountText;

    Item item;
    UIManager uIManager;

    public void Init(Item _item, UIManager _uIManager, SortedDictionary<Item, int> inventory)
    {   
        item = _item;
        ItemNameText.text = item.name;
        TypeText.text = item.itemType.ToString();
        WeightText.text = item.weight.ToString();
        AmountText.text = inventory[item].ToString();
        uIManager = _uIManager;
    }

    public void UIClick()
    {   
        uIManager.InventoryItemClick(item);
    }
}
