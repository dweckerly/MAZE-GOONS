using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
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
        if (InventoryCanvas.activeSelf)
        {
            freeLook.m_XAxis.m_MaxSpeed = 300;
            freeLook.m_YAxis.m_MaxSpeed = 2;
            InventoryCanvas.SetActive(false);
        }
        else 
        {
            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            InventoryCanvas.SetActive(true);
        }        
    }
}
