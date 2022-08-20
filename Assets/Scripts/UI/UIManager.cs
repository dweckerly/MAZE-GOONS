using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public CinemachineFreeLook freeLook;
    public GameObject UICanvas;
    public Transform ViewPortContentContainer;
    public GameObject ItemDisplayPrefab;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI brawnText;
    public TextMeshProUGUI brainsText;
    public TextMeshProUGUI gutsText;
    public TextMeshProUGUI guileText;

    public GameObject LootUI;
    public Transform LootContentContainer;
    public GameObject LootItemDisplayPrefab;
    private Loot loot;

    
    void Start()
    {
        playerStateMachine.InputReader.OpenInventoryEvent += OpenInventory;
        playerStateMachine.Inventory.AddItemEvent += UpdateInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent += ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent += ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip += EquipWeapon;
        playerStateMachine.Interacter.OnInteractEventWithUI += OpenInteractionUI;

        brawnText.text = playerStateMachine.Attributes.GetStat(Attribute.Brawn).ToString();
        brainsText.text = playerStateMachine.Attributes.GetStat(Attribute.Brains).ToString();
        gutsText.text = playerStateMachine.Attributes.GetStat(Attribute.Guts).ToString();
        guileText.text = playerStateMachine.Attributes.GetStat(Attribute.Guile).ToString();
    }

    void OnDestroy()
    {
        playerStateMachine.InputReader.OpenInventoryEvent -= OpenInventory;
        playerStateMachine.Inventory.AddItemEvent -= UpdateInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent -= ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent -= ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip -= EquipWeapon;
        playerStateMachine.Interacter.OnInteractEventWithUI -= OpenInteractionUI;
    }

    private void UpdateInventory(SortedDictionary<Item, int> inventory)
    {
        RemoveContainerChildren(ViewPortContentContainer);
        foreach (Item item in inventory.Keys)
        {
            GameObject go = Instantiate(ItemDisplayPrefab, ViewPortContentContainer);
            go.GetComponent<InventoryItemDisplay>().Init(item, this);
        }
    }

    private void OpenInventory()
    {
        if (UICanvas.activeSelf)
        {
            freeLook.m_XAxis.m_MaxSpeed = 300;
            freeLook.m_YAxis.m_MaxSpeed = 2;
            UICanvas.SetActive(false);
        }
        else 
        {
            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            UICanvas.SetActive(true);
        }        
    }

    public void InventoryItemClick(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Armor:
                playerStateMachine.ArmorHandler.CheckArmor((Armor)item);
                break;
            case ItemType.Weapon:
                playerStateMachine.WeaponHandler.EquipWeapon((Weapon)item);
                break;
            default:
                break;
        }    
    }

    public void ArmorEquip(Armor armor)
    {
        armorText.text = playerStateMachine.ArmorHandler.CalculateArmorValue().ToString();
    }

    public void ArmorUnEquip(Armor armor)
    {
        armorText.text = playerStateMachine.ArmorHandler.CalculateArmorValue().ToString();
    }

    public void EquipWeapon()
    {
        damageText.text = playerStateMachine.WeaponHandler.currentWeapon.weaponDamage.ToString();
    }

    public void OpenInteractionUI(Loot _loot)
    {
        playerStateMachine.InputReader.UIOpen = true;
        playerStateMachine.InputReader.UnlockCursor();
        loot = _loot;
        UpdateLootUI();
        LootUI.SetActive(true);
    }

    private void UpdateLootUI()
    {
        RemoveContainerChildren(LootContentContainer);
        foreach (Item item in loot.items)
        {
            GameObject go = Instantiate(LootItemDisplayPrefab, LootContentContainer);
            go.GetComponent<LootItemDisplay>().Init(item, this);
        }
    }

    private void RemoveContainerChildren(Transform container)
    {
        int children = container.childCount;
        if (children > 0)
        {
            for (int i = 0; i < children; i++)
            {
                Destroy(LootContentContainer.GetChild(i).gameObject);
            }
        }
    }

    public void LootItemClick(Item item)
    {
        playerStateMachine.Inventory.AddItem(item);
        loot.items.Remove(item);
        UpdateLootUI();
        if (loot.items.Count == 0) CloseLootUI();
    }

    public void TakeAllLoot()
    {
        foreach (Item item in loot.items)
        {
            playerStateMachine.Inventory.AddItem(item);
        }
        loot.items.Clear();
        CloseLootUI();
    }

    public void CloseLootUI()
    {
        playerStateMachine.InputReader.UIOpen = false;
        playerStateMachine.InputReader.LockCursor();
        LootUI.SetActive(false);
        if (loot != null) loot.CanInteract = true;
    }
}
