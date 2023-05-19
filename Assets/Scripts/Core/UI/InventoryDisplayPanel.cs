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
    public ItemDetails itemDetails;
    public GameObject BtnContainer;
    public UIManager UIManager;

    private Item selectedItem;

    private int panelIndex = 0;

    private void Start() 
    {
        ClearDetails();
        PopulatePanel();
        inventory.AddItemEvent += PopulatePanel;
    }

    private void OnDestroy() 
    {
        inventory.AddItemEvent -= PopulatePanel;
    }

    public void NextPanel()
    {
        ClearDetails();
        panelIndex++;
        if (panelIndex >= panels.Length) panelIndex = 0;
        PopulatePanel();
    }

    public void PreviousPanel()
    {
        ClearDetails();
        panelIndex--;
        if (panelIndex < 0) panelIndex = panels.Length - 1;
        PopulatePanel();
    }

    private void PopulatePanel()
    {
        ClearAllIcons();
        panelTitle.text = panels[panelIndex].title;
        int i = 0;
        foreach(KeyValuePair<Item, int> item in inventory.GetAllOfItemType(panels[panelIndex].itemType))
        {
            icons[i].item = item.Key;
            if (item.Value > 1) icons[i].ShowCountDisplay(item.Value);
            icons[i].ShowIcon();
            if (icons[i].item == selectedItem) icons[i].Selected();
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
        if (icons[index].item != null)
        {
            selectedItem = icons[index].item;
            itemDetails.gameObject.SetActive(true);
            BtnContainer.gameObject.SetActive(true);
            itemDetails.ShowDetails(selectedItem);
            PopulatePanel();
        }
    }

    public void ActionBtnClick()
    {
        UIManager.InventoryItemClick(selectedItem);
        if (selectedItem.itemType == ItemType.Consumable) ClearDetails();
        else itemDetails.ShowDetails(selectedItem);
        PopulatePanel();
    }

    public void DropItemClick()
    {
        UIManager.DropItem(selectedItem);
        if (!inventory.inventory.ContainsKey(selectedItem)) ClearDetails();
        PopulatePanel();
    }

    private void ClearDetails()
    {
        selectedItem = null;
        BtnContainer.gameObject.SetActive(false);
        itemDetails.gameObject.SetActive(false);
    }
}
