using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text props;
    public TMP_Text description;

    public void ShowDetails(Item item)
    {
        itemName.text = item.name;
        description.text = item.description;
    }
}
