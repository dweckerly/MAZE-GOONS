using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Animator Animator;
    private Interactable Interaction;
    public TextMeshProUGUI interactableMessage;

    public Animator TargetPanelAnimator;
    
    void Start()
    {
        playerStateMachine.InputReader.OpenInventoryEvent += OpenInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent += ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent += ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip += EquipWeapon;
        playerStateMachine.Interacter.OnDetectInteractableEvent += ShowInteractablePrompt;
        playerStateMachine.Interacter.OnInteractEventWithUI += OpenInteractionUI;
        playerStateMachine.Targeter.TargetAction += OnTarget;
        playerStateMachine.InputReader.PauseEvent += OnPause;
        playerStateMachine.Attributes.OnDie += OnDeath;

        brawnText.text = playerStateMachine.Attributes.GetStat(Attribute.Brawn).ToString();
        brainsText.text = playerStateMachine.Attributes.GetStat(Attribute.Brains).ToString();
        gutsText.text = playerStateMachine.Attributes.GetStat(Attribute.Guts).ToString();
        guileText.text = playerStateMachine.Attributes.GetStat(Attribute.Guile).ToString();
    }

    void OnDestroy()
    {
        playerStateMachine.InputReader.OpenInventoryEvent -= OpenInventory;
        playerStateMachine.ArmorHandler.EquipArmorEvent -= ArmorEquip;
        playerStateMachine.ArmorHandler.UnEquipArmorEvent -= ArmorUnEquip;
        playerStateMachine.WeaponHandler.OnEquip -= EquipWeapon;
        playerStateMachine.Interacter.OnDetectInteractableEvent -= ShowInteractablePrompt;
        playerStateMachine.Interacter.OnInteractEventWithUI -= OpenInteractionUI;
        playerStateMachine.Targeter.TargetAction -= OnTarget;
        playerStateMachine.InputReader.PauseEvent -= OnPause;
        playerStateMachine.Attributes.OnDie -= OnDeath;
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
                playerStateMachine.InputReader.UIOpen = false;
            }
            else 
            {
                if (LootUI.activeSelf) CloseLootUI();
                playerStateMachine.InputReader.UIOpen = true;
                playerStateMachine.InputReader.UnlockCursor();
                freeLook.m_XAxis.m_MaxSpeed = 0;
                freeLook.m_YAxis.m_MaxSpeed = 0;
                UICanvas.SetActive(true);
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
    }

    public void ArmorEquip(Armor armor)
    {
        //armorText.text = playerStateMachine.ArmorHandler.CalculateArmorValue().ToString();
    }

    public void ArmorUnEquip(Armor armor)
    {
        //armorText.text = playerStateMachine.ArmorHandler.CalculateArmorValue().ToString();
    }

    public void EquipWeapon()
    {
        //damageText.text = playerStateMachine.WeaponHandler.mainHandWeapon.weaponDamage.ToString();
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

    private void UpdateLootUI()
    {
        RemoveContainerChildren(LootContentContainer);
        foreach (Item item in loot.items)
        {
            GameObject go = Instantiate(LootItemDisplayPrefab, LootContentContainer);
            go.GetComponent<LootItemDisplay>().Init(item, this);
        }
    }

    public void LootItemClick(Item item)
    {
        playerStateMachine.Inventory.AddItem(item);
        loot.items.Remove(item);
        switch (item.itemType)
        {
            case ItemType.Armor:
                loot.enemyStateMachine.ArmorHandler.CheckArmor((Armor)item);
                break;
            case ItemType.Shield:
                loot.enemyStateMachine.WeaponHandler.EquipShield((Shield)item);
                break;
            case ItemType.Weapon:
                loot.enemyStateMachine.WeaponHandler.UnEquipMainHand();
                break;
            default:
                break;
        }
        UpdateLootUI();
        if (loot.items.Count == 0) CloseLootUI();
    }

    public void TakeAllLoot()
    {
        foreach (Item item in loot.items)
        {
            playerStateMachine.Inventory.AddItem(item);
        }
        loot.enemyStateMachine.WeaponHandler.UnEquipAllWeapons();
        loot.enemyStateMachine.ArmorHandler.UnEquipAllArmor();
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
}
