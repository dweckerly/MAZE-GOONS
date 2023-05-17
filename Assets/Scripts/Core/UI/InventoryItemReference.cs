using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemReference : MonoBehaviour
{
    public Item item;
    public Image iconBackGround;
    public Image itemIcon;

    private Color iconBGDefaultColor = new Color(0.5176471f, 0.5176471f, 0.5176471f, 0.3921569f);
    private Color iconBGFilledColor = new Color(0.5990566f, 1f, 0.9917222f, 1f);
    private Color itemIconDefaultColor = new Color(1, 1, 1, 0);
    private Color itemIconFilledColor = new Color(1, 1, 1, 1);

    public void ShowIcon()
    {
        itemIcon.sprite = item.icon;
        itemIcon.color = itemIconFilledColor;
        iconBackGround.color = iconBGDefaultColor;
    }

    public void Clear()
    {
        item = null;
        iconBackGround.color = iconBGDefaultColor;
        itemIcon.sprite = null;
        itemIcon.color = itemIconDefaultColor;
    }

    public void Selected()
    {
        iconBackGround.color = iconBGFilledColor;
    }
}
