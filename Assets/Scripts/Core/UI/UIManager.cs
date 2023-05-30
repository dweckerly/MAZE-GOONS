using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public CinemachineFreeLook freeLook;
    public GameObject UICanvas;
    public GameObject PauseCanvas;
    public GameObject GameOverCanvas;
    public GameObject OfferingCanvas;
    public TextMeshProUGUI brawnText;
    public TextMeshProUGUI brainsText;
    public TextMeshProUGUI gutsText;
    public TextMeshProUGUI guileText;
    public TMP_Text attackText;
    public TMP_Text defenseText;

    public GameObject LootUI;
    public Transform LootContentContainer;
    public GameObject LootItemDisplayPrefab;
    private Loot loot;

    public Animator Animator;
    private Interactable Interaction;
    public TextMeshProUGUI interactableMessage;

    public Animator TargetPanelAnimator;

    public Item[] hotKeyItems = new Item[4];
    public Image[] hotKeyImages;
    public Sprite baseUISprite;

    public event Action OnInventoryOpen;
    
    void Start()
    {
        playerStateMachine.InputReader.OpenInventoryEvent += OpenInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent += ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent += ArmorEquip;
        playerStateMachine.WeaponHandler.OnEquip += EquipWeapon;
        playerStateMachine.WeaponHandler.OnEquipShield += ShieldEquip;
        playerStateMachine.WeaponHandler.OnUnEquipShield += ShieldEquip;
        playerStateMachine.Interacter.OnDetectInteractableEvent += ShowInteractablePrompt;
        playerStateMachine.Interacter.OnInteractEventWithUI += OpenInteractionUI;
        playerStateMachine.Interacter.OnMakeOffering += OpenOfferingUI;
        playerStateMachine.Targeter.TargetAction += OnTarget;
        playerStateMachine.InputReader.PauseEvent += OnPause;
        playerStateMachine.Attributes.OnDie += OnDeath;

        playerStateMachine.InputReader.HotKey1Event += HotKey1;
        playerStateMachine.InputReader.HotKey2Event += HotKey2;
        playerStateMachine.InputReader.HotKey3Event += HotKey3;
        playerStateMachine.InputReader.HotKey4Event += HotKey4;
        UpdateStats();
    }

    public void UpdateStats()
    {
        brawnText.text = playerStateMachine.Attributes.GetStat(Attribute.Brawn).ToString();
        brainsText.text = playerStateMachine.Attributes.GetStat(Attribute.Brains).ToString();
        gutsText.text = playerStateMachine.Attributes.GetStat(Attribute.Guts).ToString();
        guileText.text = playerStateMachine.Attributes.GetStat(Attribute.Guile).ToString();
    }

    void OnDestroy()
    {
        playerStateMachine.InputReader.OpenInventoryEvent -= OpenInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent -= ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent -= ArmorEquip;
        playerStateMachine.WeaponHandler.OnEquip -= EquipWeapon;
        playerStateMachine.WeaponHandler.OnEquipShield -= ShieldEquip;
        playerStateMachine.WeaponHandler.OnUnEquipShield -= ShieldEquip;
        playerStateMachine.Interacter.OnDetectInteractableEvent -= ShowInteractablePrompt;
        playerStateMachine.Interacter.OnInteractEventWithUI -= OpenInteractionUI;
        playerStateMachine.Interacter.OnMakeOffering -= OpenOfferingUI;
        playerStateMachine.Targeter.TargetAction -= OnTarget;
        playerStateMachine.InputReader.PauseEvent -= OnPause;
        playerStateMachine.Attributes.OnDie -= OnDeath;

        playerStateMachine.InputReader.HotKey1Event -= HotKey1;
        playerStateMachine.InputReader.HotKey2Event -= HotKey2;
        playerStateMachine.InputReader.HotKey3Event -= HotKey3;
        playerStateMachine.InputReader.HotKey4Event -= HotKey4;
    }

    private void OnTarget()
    {
        bool show = TargetPanelAnimator.GetBool("ShowPanels");
        TargetPanelAnimator.SetBool("ShowPanels", !show);
    }

    private void ShowInteractablePrompt(Interactable interactable)
    {
        if (interactable == null || !interactable.CanInteract)
        {
            Interaction = interactable;
            Animator.SetBool("ShowPrompt", false);
            return;
        }
        if (interactable != Interaction)
        {
            Interaction = interactable;
            switch (Interaction.type)
            {
                case InteractableType.Body:
                interactableMessage.text = "Loot";
                    break;
                case InteractableType.Chest:
                    interactableMessage.text = "Open Chest";
                    break;
                case InteractableType.Door:
                    if(((Door)interactable).isOpen)
                        interactableMessage.text = "Close Door";
                    else
                        interactableMessage.text = "Open Door";
                    break;
                case InteractableType.Lever:
                    interactableMessage.text = "Pull Lever";
                    break;
                case InteractableType.PickUp:
                    ItemPickup pickup = (ItemPickup) Interaction;
                    interactableMessage.text = "Pick Up " + pickup.item.itemName;
                    break;
                case InteractableType.Gold:
                    interactableMessage.text = "Take Coin Purse";
                    break;
                case InteractableType.Statue:
                    interactableMessage.text = "Make Offering";
                    break;
            }
            Animator.SetBool("ShowPrompt", true);
        }
    }

    private void RemoveContainerChildren(Transform container)
    {
        int children = container.childCount;
        if (children > 0)
        {
            for (int i = 0; i < children; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    private void OpenInventory()
    {
        if(!PauseCanvas.activeSelf && !GameOverCanvas.activeSelf)
        {
            if (UICanvas.activeSelf)
            {
                CloseInventoryUI();
                playerStateMachine.InputReader.LockCursor();
                playerStateMachine.InputReader.UIOpen = false;
            }
            else 
            {
                if (LootUI.activeSelf) CloseLootUI();
                CloseOfferingUI();
                UpdateStats();
                playerStateMachine.Inventory.goldText.text = playerStateMachine.Inventory.gold.ToString();
                playerStateMachine.InputReader.UIOpen = true;
                playerStateMachine.InputReader.UnlockCursor();
                freeLook.m_XAxis.m_MaxSpeed = 0;
                freeLook.m_YAxis.m_MaxSpeed = 0;
                UICanvas.SetActive(true);
                OnInventoryOpen?.Invoke();
            }
        }
    }

    public void InventoryItemClick(Item item)
    {
        switch (item.itemType)
        {
            case ItemType.Armor:
                playerStateMachine.ArmorHandler.CheckArmor((Armor)item);
                break;
            case ItemType.Consumable:
                ((Consumable) item).Consume(playerStateMachine);
                playerStateMachine.Inventory.RemoveItem(item);
                if (!playerStateMachine.Inventory.inventory.ContainsKey(item)) CheckHotKeys(item);
                break;
            case ItemType.Shield:
                playerStateMachine.WeaponHandler.EquipShield((Shield)item);
                break;
            case ItemType.Weapon:
                playerStateMachine.WeaponHandler.EquipWeapon((Weapon)item);
                break;
            default:
                break;
        }    
    }

    public void DropItem(Item item)
    {
        Instantiate(item.pickup, playerStateMachine.gameObject.transform.position, Quaternion.identity);
        if (item is Equippable && ((Equippable) item).equipped)
        {
            switch (item.itemType)
            {
                case ItemType.Armor:
                    playerStateMachine.ArmorHandler.CheckArmor((Armor)item);
                    break;
                case ItemType.Shield:
                    playerStateMachine.WeaponHandler.EquipShield((Shield)item);
                    break;
                case ItemType.Weapon:
                    playerStateMachine.WeaponHandler.EquipWeapon((Weapon)item);
                    break;
                default:
                    break;
            }
        }
        playerStateMachine.Inventory.RemoveItem(item);
        if (!playerStateMachine.Inventory.inventory.ContainsKey(item)) CheckHotKeys(item);
    }

    public void ArmorEquip(Armor armor)
    {
        defenseText.text = "DEF " + playerStateMachine.Attributes.DamageReduction.ToString();
    }

    public void ShieldEquip()
    {
        attackText.text = "ATK " + playerStateMachine.WeaponHandler.mainHandWeapon.weaponDamage.ToString();
        defenseText.text = "DEF " + playerStateMachine.Attributes.DamageReduction.ToString();

    }

    public void EquipWeapon()
    {
        attackText.text = "ATK " + playerStateMachine.WeaponHandler.mainHandWeapon.weaponDamage.ToString();
    }

    public void OpenInteractionUI(Loot _loot)
    {
        if(!PauseCanvas.activeSelf && !UICanvas.activeSelf && !GameOverCanvas.activeSelf)
        {
            playerStateMachine.InputReader.UIOpen = true;
            playerStateMachine.InputReader.UnlockCursor();
            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            loot = _loot;
            UpdateLootUI();
            LootUI.SetActive(true);
        }
    }

    private void OpenOfferingUI()
    {
        if (!PauseCanvas.activeSelf && !UICanvas.activeSelf && !GameOverCanvas.activeSelf)
        {
            playerStateMachine.InputReader.UIOpen = true;
            playerStateMachine.InputReader.UnlockCursor();
            OfferingCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void CloseOfferingUI()
    {
        playerStateMachine.InputReader.LockCursor();
        OfferingCanvas.SetActive(false);
    }

    private void UpdateLootUI()
    {
        RemoveContainerChildren(LootContentContainer);
        foreach (LootItem item in loot.items)
        {
            GameObject go = Instantiate(LootItemDisplayPrefab, LootContentContainer);
            go.GetComponent<LootItemDisplay>().Init(item, this);
        }
    }

    public void LootItemClick(LootItem item)
    {
        playerStateMachine.Inventory.AddItem(item.item);
        loot.items.Remove(item);
        if (item.prefab != null) item.prefab.SetActive(false);
        UpdateLootUI();
        if (loot.items.Count == 0) CloseLootUI();
    }

    public void TakeAllLoot()
    {
        foreach (LootItem item in loot.items)
        {
            playerStateMachine.Inventory.AddItem(item.item);
            if (item.prefab != null) item.prefab.SetActive(false);
        }
        loot.items.Clear();
        CloseLootUI();
    }

    public void CloseLootUI()
    {
        playerStateMachine.InputReader.UIOpen = false;
        playerStateMachine.InputReader.LockCursor();
        freeLook.m_XAxis.m_MaxSpeed = 300;
        freeLook.m_YAxis.m_MaxSpeed = 2;
        LootUI.SetActive(false);
        if (loot != null) loot.EnableLoot();
        if (loot.items.Count == 0) loot.DisableLoot();
    }

    public void OnPause()
    {
        if (!GameOverCanvas.activeSelf)
        {
            if (PauseCanvas.activeSelf) 
            {
                playerStateMachine.InputReader.UIOpen = false;
                playerStateMachine.InputReader.LockCursor();
                PauseCanvas.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                if (LootUI.activeSelf) CloseLootUI();
                CloseInventoryUI();
                CloseOfferingUI();
                playerStateMachine.InputReader.UIOpen = true;
                playerStateMachine.InputReader.UnlockCursor();
                PauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    void CloseInventoryUI()
    {
        freeLook.m_XAxis.m_MaxSpeed = 300;
        freeLook.m_YAxis.m_MaxSpeed = 2;
        UICanvas.SetActive(false);
    }

    void OnDeath()
    {
        if (LootUI.activeSelf) CloseLootUI();
        CloseInventoryUI();
        playerStateMachine.InputReader.UIOpen = true;
        playerStateMachine.InputReader.UnlockCursor();
        GameOverCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetHotKey(Item item, int index)
    {
        hotKeyItems[index] = item;
        hotKeyImages[index].sprite = item.icon;
    }

    private void CheckHotKeys(Item item)
    {
        for(int i = 0; i < hotKeyItems.Length; i++)
        {
            if (hotKeyItems[i] == item)
            {
                hotKeyItems[i] = null;
                hotKeyImages[i].sprite = baseUISprite;
            }
        }
    }

    private void HotKey1()
    {
        if (hotKeyItems[0] != null && !UICanvas.activeSelf) InventoryItemClick(hotKeyItems[0]);
    }

    private void HotKey2()
    {
        if (hotKeyItems[1] != null && !UICanvas.activeSelf) InventoryItemClick(hotKeyItems[1]);
    }

    private void HotKey3()
    {
        if (hotKeyItems[2] != null && !UICanvas.activeSelf) InventoryItemClick(hotKeyItems[2]);
    }

    private void HotKey4()
    {
        if (hotKeyItems[3] != null && !UICanvas.activeSelf) InventoryItemClick(hotKeyItems[3]);
    }
}
