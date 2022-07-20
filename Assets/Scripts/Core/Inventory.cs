using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

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
