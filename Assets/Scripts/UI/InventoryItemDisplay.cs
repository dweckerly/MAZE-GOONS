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

    Item item;
    UIManager uIManager;

    public void Init(Item _item, UIManager _uIManager)
    {   
        item = _item;
        ItemNameText.text = item.name;
        TypeText.text = item.itemType.ToString();
        WeightText.text = item.weight.ToString();
        ValueText.text = item.value.ToString();
        uIManager = _uIManager;
    }

    public void UIClick()
    {   
        uIManager.InventoryItemClick(item);
    }
}
