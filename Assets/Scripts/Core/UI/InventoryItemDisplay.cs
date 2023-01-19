using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI TypeText;
    public TextMeshProUGUI WeightText;
    public TextMeshProUGUI ValueText;
    public TextMeshProUGUI AmountText;

    Item item;
    UIManager uIManager;

    public void Init(Item _item, UIManager _uIManager, SortedDictionary<Item, int> inventory)
    {   
        item = _item;
        ItemNameText.text = item.itemName;
        TypeText.text = item.itemType.ToString();
        WeightText.text = item.weight.ToString();
        ValueText.text = item.value.ToString();
        AmountText.text = inventory[item].ToString();
        uIManager = _uIManager;
    }

    public void UIClick()
    {   
        uIManager.InventoryItemClick(item);
    }
}