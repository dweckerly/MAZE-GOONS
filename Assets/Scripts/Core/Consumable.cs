using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    HealthPotion,
    StaminaPotion
}


[CreateAssetMenu(fileName = "Consumable", menuName = "Items/New Consumable", order = 4)]
public class Consumable : Item
{
    public override ItemType itemType => ItemType.Consumable;
    public ConsumableType consumableType;
    public int effectAmount = 10;

    public void Consume(PlayerStateMachine stateMachine)
    {
        switch (consumableType)
        {
            case ConsumableType.HealthPotion:
                stateMachine.Attributes.ChangeHP(effectAmount);
                break;
            case ConsumableType.StaminaPotion:
                stateMachine.Attributes.HealStamina(effectAmount);
                break;
            default:
                break;
        }
    }
}
