using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OfferingUI : MonoBehaviour
{
    const int STAT_INCREASE_PRICE_FACTOR = 100;
    public PlayerStateMachine playerStateMachine;

    public TMP_Text brawnVal;
    public TMP_Text brainsVal;
    public TMP_Text gutsVal;
    public TMP_Text guileVal;

    public TMP_Text brawnPrice;
    public TMP_Text brainsPrice;
    public TMP_Text gutsPrice;
    public TMP_Text guilePrice;

    public TMP_Text gold;

    public TMP_Text errorMessage;

    private int tempBrawn;
    private int tempBrains;
    private int tempGuts;
    private int tempGuile;
    private int tempGold;

    private void Start()
    {
        errorMessage.text = "";
        GetStatReferences();
        SetStatValues();
    }

    private void GetStatReferences()
    {
        tempBrawn = playerStateMachine.Attributes.brawn;
        tempBrains = playerStateMachine.Attributes.brains;
        tempGuts = playerStateMachine.Attributes.guts;
        tempGuile = playerStateMachine.Attributes.guile;
        tempGold = playerStateMachine.Inventory.gold;
    }

    private void SetStatValues()
    {
        brawnVal.text = tempBrawn.ToString();
        brainsVal.text = tempBrains.ToString();
        gutsVal.text = tempGuts.ToString();
        guileVal.text = tempGuile.ToString();

        brawnPrice.text = (tempBrawn * STAT_INCREASE_PRICE_FACTOR).ToString();
        brainsPrice.text = (tempBrains * STAT_INCREASE_PRICE_FACTOR).ToString();
        gutsPrice.text = (tempGuts * STAT_INCREASE_PRICE_FACTOR).ToString();
        guilePrice.text = (tempGuile * STAT_INCREASE_PRICE_FACTOR).ToString();

        gold.text = tempGold.ToString();
    }

    public void BuyBrawn()
    {
        int brawnPrice = tempBrawn * STAT_INCREASE_PRICE_FACTOR;
        if (brawnPrice > tempGold) 
        {
            errorMessage.text = "You do not have enough gold.";
            return;
        }
        tempGold -= brawnPrice;
        tempBrawn += 1;
        SetStatValues();
    }

    public void BuyBrains()
    {
        int brawnPrice = tempBrains * STAT_INCREASE_PRICE_FACTOR;
        if (brawnPrice > tempGold)
        {
            errorMessage.text = "You do not have enough gold.";
            return;
        }
        tempGold -= brawnPrice;
        tempBrains += 1;
        SetStatValues();
    }

    public void BuyGuts()
    {
        int gutsPrice = tempGuts * STAT_INCREASE_PRICE_FACTOR;
        if (gutsPrice > tempGold)
        {
            errorMessage.text = "You do not have enough gold.";
            return;
        }
        tempGold -= gutsPrice;
        tempGuts += 1;
        SetStatValues();
    }

    public void BuyGuile()
    {
        int guilePrice = tempGuile * STAT_INCREASE_PRICE_FACTOR;
        if (guilePrice > tempGold)
        {
            errorMessage.text = "You do not have enough gold.";
            return;
        }
        tempGold -= guilePrice;
        tempGuile += 1;
        SetStatValues();
    }

    public void ConfirmOffering()
    {
        errorMessage.text = "";
        playerStateMachine.Attributes.brawn = tempBrawn;
        playerStateMachine.Attributes.brains = tempBrains;
        playerStateMachine.Attributes.guts = tempGuts;
        playerStateMachine.Attributes.guile = tempGuile;
        playerStateMachine.Inventory.gold = tempGold;
        playerStateMachine.Attributes.InstantiateStats();
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void CancelOffering()
    {
        errorMessage.text = "";
        GetStatReferences();
        SetStatValues();
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
