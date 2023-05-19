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

    Item item;
    UIManager uIManager;

    public void Init(Item _item, UIManager _uIManager)
    {
        item = _item;
        ItemIcon.sprite = item.icon;
        ItemNameText.text = item.itemName;
        WeightText.text = item.weight.ToString();
        uIManager = _uIManager;
    }

    public void UIClick()
    {
        uIManager.LootItemClick(item);
    }
}
