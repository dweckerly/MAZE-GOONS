using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public Canvas notificationCanvas;
    public TMP_Text notificationText;
    public Animator Animator;

    private float displayTime = 1f;

    private void Notify()
    {
        Animator.Play("ShowNotification");
        StartCoroutine(DisplayNotification());
    }

    public void NotifyGold(int amount)
    {
        if (amount > 0)
            notificationText.text = "+ " + amount.ToString() + " gold";
        else
           notificationText.text = "- " + amount.ToString() + " gold";

        Notify();
    }

    private IEnumerator DisplayNotification()
    {
        yield return new WaitForSeconds(displayTime);
        Animator.Play("HideNotification");
    }
}
