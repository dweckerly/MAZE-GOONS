using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PanelType
{
    public string title;
    public ItemType itemType;
}

public class InventoryDisplayPanel : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text panelTitle;
    public InventoryItemReference[] icons;
    public PanelType[] panels;

    private Item selectedItem;

    private int panelIndex = 0;

    private void Start() 
    {
        PopulatePanel();
        inventory.AddItemEvent += PopulatePanel;
    }

    private void OnDestroy() 
    {
        inventory.AddItemEvent -= PopulatePanel;
    }

    public void NextPanel()
    {
        panelIndex++;
        if (panelIndex >= panels.Length) panelIndex = 0;
        PopulatePanel();
    }

    public void PreviousPanel()
    {
        panelIndex--;
        if (panelIndex < 0) panelIndex = panels.Length - 1;
        PopulatePanel();
    }

    private void PopulatePanel()
    {
        ClearAllIcons();
        panelTitle.text = panels[panelIndex].title;
        int i = 0;
        foreach(Item item in inventory.GetAllOfItemType(panels[panelIndex].itemType))
        {
            icons[i].item = item;
            icons[i].ShowIcon();
            i++;
        }
    }

    private void ClearAllIcons()
    {
        for(int i = 0; i < icons.Length; i++)
        {
            icons[i].Clear();
        }
    }

    public void IconItemClick(int index)
    {
        PopulatePanel();
        selectedItem = icons[index].item;
        icons[index].Selected();
    }
}
