using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventoryIcon
{
    public Image iconBackGround;
    public Image itemIcon;
}

public class InventoryDisplayPanel : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text panelTitle;
    public InventoryIcon[] icons;
}
