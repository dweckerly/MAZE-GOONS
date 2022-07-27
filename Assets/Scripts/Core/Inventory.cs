using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int gold = 0;
    
    private int goldCounter = 0;
    private int goldCounterMax = 0;
    private int currentGold = 0;

    public List<Item> items = new List<Item>();
    public TextMeshProUGUI goldText;

    private void Start() 
    {
        goldText.text = gold.ToString();    
    }

    public void UpdateGold(int amount)
    {
        goldCounter = 0;
        goldCounterMax = amount;
        currentGold = gold;
        gold += goldCounterMax;
        StartCoroutine(GoldCounter());
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
