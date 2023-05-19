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
    public float carryCapacity;
    public TextMeshProUGUI weightText;
    public RectTransform weightRect;

    public delegate void AddItemCallback(SortedDictionary<Item, int> inventory);
    public event Action AddItemEvent;

    private void Start() 
    {
        if (goldText != null) goldText.text = gold.ToString();
        UpdateWeightDisplay();
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
        UpdateWeightDisplay();
        AddItemEvent?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        inventory[item]--;
        UpdateWeightDisplay();
        if (inventory[item] == 0) inventory.Remove(item);
    }

    private void UpdateWeightDisplay()
    {
        if (weightText != null) weightText.text = WeightOfItems() + " / " + carryCapacity;
        if (weightRect != null) weightRect.localScale = new Vector3(WeightOfItems() / carryCapacity, 1f, 1f);
    }

    private float WeightOfItems()
    {
        float weight = 0;
        foreach (Item item in inventory.Keys)
        {
            weight += (item.weight * inventory[item]);
        }
        return weight;
    }

    public SortedDictionary<Item, int> GetAllOfItemType(ItemType itemType)
    {
        SortedDictionary<Item, int> items = new SortedDictionary<Item, int>(new ItemComparer());
        foreach(Item item in inventory.Keys)
        {
            if (item.itemType == itemType)
            {
                items.Add(item, inventory[item]);
            } 
        }
        return items;
    }
}
