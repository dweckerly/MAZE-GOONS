using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public abstract string itemType { get; }
    public float weight;
    public int value;

    public abstract void UIClick();

}
