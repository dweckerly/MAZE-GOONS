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

    public void Init(Item item)
    {
        ItemNameText.text = item.name;
        TypeText.text = item.itemType;
        WeightText.text = item.weight.ToString();
        ValueText.text = item.value.ToString();
    }
}
