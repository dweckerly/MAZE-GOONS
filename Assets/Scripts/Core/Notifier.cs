using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public GameObject Notification;
    string message;

    private void Notify()
    {
        GameObject go = Instantiate(Notification, gameObject.transform);
        go.GetComponent<Notification>().Notify(message);
    }

    public void NotifyGold(int amount)
    {
        if (amount > 0)
            message = "+ " + amount.ToString() + " gold";
        else
            message = "- " + amount.ToString() + " gold";
        Notify();
    }
}
