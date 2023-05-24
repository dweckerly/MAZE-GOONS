using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootItemDisplay : MonoBehaviour
{
    public Image ItemIcon;
    public TMP_Text ItemNameText;
    public TMP_Text WeightText;

    LootItem item;
    UIManager uIManager;

    public void Init(LootItem _item, UIManager _uIManager)
    {
        item = _item;
        ItemIcon.sprite = item.item.icon;
        ItemNameText.text = item.item.itemName;
        WeightText.text = item.item.weight.ToString() + " lbs.";
        uIManager = _uIManager;
    }

    public void UIClick()
    {
        uIManager.LootItemClick(item);
    }
}
