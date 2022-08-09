using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIManager : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public CinemachineFreeLook freeLook;
    public GameObject InventoryCanvas;
    public Transform ViewPortContentContainer;
    public GameObject ItemDisplayPrefab;
    
    void Start()
    {
        playerStateMachine.InputReader.OpenInventoryEvent += OpenInventory;
        playerStateMachine.Inventory.AddItemEvent += AddInventoryItem;
    }

    void OnDestroy()
    {
        playerStateMachine.InputReader.OpenInventoryEvent -= OpenInventory;
        playerStateMachine.Inventory.AddItemEvent -= AddInventoryItem;
    }

    private void AddInventoryItem(Item item)
    {
        GameObject go = Instantiate(ItemDisplayPrefab, ViewPortContentContainer);
        go.GetComponent<InventoryItemDisplay>().Init(item, this);
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

    public void InventoryItemClick(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Armor:
                playerStateMachine.ArmorHandler.EquipArmor((Armor)item);
                break;
            case ItemType.Weapon:
                playerStateMachine.WeaponHandler.EquipWeapon((Weapon)item);
                break;
            default:
                break;
        }
        
    }
}
