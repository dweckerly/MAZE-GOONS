using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public GameObject Notification;
    string message;

    private void Notify(bool sfx)
    {
        GameObject go = Instantiate(Notification, gameObject.transform);
        go.GetComponent<Notification>().Notify(message, sfx);
    }

    public void NotifyGold(int amount)
    {
        if (amount > 0)
            message = "+ " + amount.ToString() + " gold";
        else
            message = "- " + amount.ToString() + " gold";
        Notify(true);
    }

    public void NofityDamageMultiple(int amount)
    {
        message = "x" + amount.ToString();
        Notify(false);
    }
}
