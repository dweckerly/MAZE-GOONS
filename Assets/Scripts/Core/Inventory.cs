using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class ItemComparer : IComparer<Item>
{
    public int Compare(Item x, Item y)
    {
        return string.Compare(x.itemName, y.itemName);
    }
}

public class Inventory : MonoBehaviour
{
    public int gold = 0;
    
    private int goldCounter = 0;
    private int goldCounterMax = 0;
    private int currentGold = 0;

    //public List<Item> items = new List<Item>();
    public SortedDictionary<Item, int> inventory = new SortedDictionary<Item, int>(new ItemComparer());
    public TextMeshProUGUI goldText;

    public delegate void AddItemCallback(SortedDictionary<Item, int> inventory);
    public event AddItemCallback AddItemEvent;

    private void Start() 
    {
        if (goldText != null) goldText.text = gold.ToString();    
    }

    public void UpdateGold(int amount)
    {
        goldCounter = 0;
        goldCounterMax = amount;
        currentGold = gold;
        gold += goldCounterMax;
        if (goldText != null) StartCoroutine(GoldCounter());
    }

    private IEnumerator GoldCounter()
    {
        while (goldCounter <= goldCounterMax)
        {
            goldCounter++;
            goldText.text = (currentGold + goldCounter).ToString();
            yield return new WaitForSeconds(0.01f);
        }
        goldText.text = gold.ToString();
    }


    public void AddItem(Item item)
    {
        if (inventory.ContainsKey(item)) inventory[item]++;
        else inventory.Add(item, 1);
        AddItemEvent?.Invoke(inventory);
    }

    public void RemoveItem(Item item)
    {
        inventory[item]--;
        if (inventory[item] == 0) inventory.Remove(item);
    }
}
