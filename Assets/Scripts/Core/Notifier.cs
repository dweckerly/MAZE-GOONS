using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public Canvas notificationCanvas;
    public TMP_Text notificationText;
    public Animator Animator;

    private void Notify()
    {
        Animator.enabled = false;
        Animator.enabled = true;
        Animator.Play("IdleNotification");
        Animator.Play("ShowNotification");
    }

    public void NotifyGold(int amount)
    {
        if (amount > 0)
            notificationText.text = "+ " + amount.ToString() + " gold";
        else
           notificationText.text = "- " + amount.ToString() + " gold";

        Notify();
    }
}
