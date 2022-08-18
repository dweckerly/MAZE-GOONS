using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public CinemachineFreeLook freeLook;
    public GameObject InventoryCanvas;
    public Transform ViewPortContentContainer;
    public GameObject ItemDisplayPrefab;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI brawnText;
    public TextMeshProUGUI brainsText;
    public TextMeshProUGUI gutsText;
    public TextMeshProUGUI guileText;
    
    void Start()
    {
        playerStateMachine.InputReader.OpenInventoryEvent += OpenInventory;
        playerStateMachine.Inventory.AddItemEvent += AddInventoryItem;
        playerStateMachine.ArmorHandler.EquipArmorEvent += ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent += ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip += EquipWeapon;

        brawnText.text = playerStateMachine.Attributes.GetStat(Attribute.Brawn).ToString();
        brainsText.text = playerStateMachine.Attributes.GetStat(Attribute.Brains).ToString();
        gutsText.text = playerStateMachine.Attributes.GetStat(Attribute.Guts).ToString();
        guileText.text = playerStateMachine.Attributes.GetStat(Attribute.Guile).ToString();
    }

    void OnDestroy()
    {
        playerStateMachine.InputReader.OpenInventoryEvent -= OpenInventory;
        playerStateMachine.Inventory.AddItemEvent -= AddInventoryItem;
        playerStateMachine.ArmorHandler.EquipArmorEvent -= ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent -= ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip -= EquipWeapon;
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

    public void ArmorEquip(Armor armor)
    {
        armorText.text = playerStateMachine.Attributes.DamageReduction.ToString();
    }

    public void ArmorUnEquip(Armor armor)
    {
        armorText.text = playerStateMachine.Attributes.DamageReduction.ToString();
    }

    public void EquipWeapon()
    {
        damageText.text = playerStateMachine.WeaponHandler.currentWeapon.weaponDamage.ToString();
    }

}
