using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int gold = 0;
    public List<Item> items = new List<Item>();
    public TextMeshProUGUI goldText;

    private void Start() 
    {
        UpdateGold(gold);    
    }

    public void UpdateGold(int amount)
    {
        gold += amount;
        goldText.text = gold.ToString();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public Item GetItem(int index)
    {
        return items[index];
    }

    public int GetItemIndex(Item item)
    {
        return items.IndexOf(item);
    }
}
