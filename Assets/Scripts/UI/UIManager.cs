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
    }

    void OnDestroy()
    {
        InputReader.OpenInventoryEvent -= OpenInventory;
    }

    private void OpenInventory()
    {
        InventoryCanvas.SetActive(!InventoryCanvas.activeSelf);
    }
}
