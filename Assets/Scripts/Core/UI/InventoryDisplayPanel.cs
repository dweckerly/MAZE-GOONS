using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private Color iconBGDefaultColor = new Color(0.5176471f, 0.5176471f, 0.5176471f, 0.3921569f);
    private Color iconBGFilledColor = new Color(0.5990566f, 1f, 0.9917222f, 1f);
    private Color itemIconDefaultColor = new Color(1, 1, 1, 0);
    private Color itemIconFilledColor = new Color(1, 1, 1, 1);

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
            icons[i].itemIcon.sprite = item.icon;
            icons[i].itemIcon.color = itemIconFilledColor;
            icons[i].iconBackGround.color = iconBGFilledColor;
            i++;
        }
    }

    private void ClearAllIcons()
    {
        for(int i = 0; i < icons.Length; i++)
        {
            icons[i].item = null;
            icons[i].iconBackGround.color = iconBGDefaultColor;
            icons[i].itemIcon.sprite = null;
            icons[i].itemIcon.color = itemIconDefaultColor;
        }
    }

    public void IconItemClick(int index)
    {
        Debug.Log(icons[index].item.name);
        Debug.Log("Clicked!");
    }
}
