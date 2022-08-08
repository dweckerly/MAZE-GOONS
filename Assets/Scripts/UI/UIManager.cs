using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InputReader InputReader;
    public Inventory Inventory;
    public GameObject InventoryCanvas;
    public Transform ViewPortContentContainer;
    public GameObject ItemDisplayPrefab;
    
    void Start()
    {
        InputReader.OpenInventoryEvent += OpenInventory;
        Inventory.AddItemEvent += AddInventoryItem;
    }

    void OnDestroy()
    {
        InputReader.OpenInventoryEvent -= OpenInventory;
    }

    private void AddInventoryItem(Item item)
    {
        GameObject go = Instantiate(ItemDisplayPrefab, ViewPortContentContainer);
        go.GetComponent<InventoryItemDisplay>().Init(item);
    }

    private void OpenInventory()
    {
        InventoryCanvas.SetActive(!InventoryCanvas.activeSelf);
    }
}
