using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemReference : MonoBehaviour
{
    public Item item;
    public Image iconBackGround;
    public Image selectedBackGround;
    public Image itemIcon;
    public GameObject countDisplay;
    public TMP_Text count; 

    private Color iconBGDefaultColor = new Color(0.5176471f, 0.5176471f, 0.5176471f, 0.3921569f);
    private Color iconBGFilledColor = new Color(0.5176471f, 0.5176471f, 0.5176471f, 1);
    private Color iconBGSelectedColor = new Color(1, 1, 1, 1);
    private Color iconBGEquippedColor = new Color(0.5990566f, 1f, 0.9917222f, 1f);
    private Color hideColor = new Color(1, 1, 1, 0);
    private Color itemIconFilledColor = new Color(1, 1, 1, 1);

    public void ShowIcon()
    {
        itemIcon.sprite = item.icon;
        itemIcon.color = itemIconFilledColor;
        iconBackGround.color = iconBGFilledColor;
        // if (item is Equippable && ((Equippable) item).equipped) 
        // {
        //     iconBackGround.color = iconBGEquippedColor;
        // }
        // else 
        // {
        //     selectedBackGround.SetActive(true);
        //     iconBackGround.color = iconBGDefaultColor;
        // }
    }

    public void ShowCountDisplay(int amount)
    {
        count.text = amount.ToString();
        countDisplay.SetActive(true);
    }

    private void HideCountDisplay()
    {
        count.text = "";
        countDisplay.SetActive(false);
    }

    public void Clear()
    {
        item = null;
        iconBackGround.color = iconBGDefaultColor;
        selectedBackGround.color = hideColor;
        itemIcon.sprite = null;
        itemIcon.color = hideColor;
        HideCountDisplay();
    }

    public void Selected()
    {
        iconBackGround.color = iconBGSelectedColor;
        selectedBackGround.color = iconBGFilledColor;
    }
}
