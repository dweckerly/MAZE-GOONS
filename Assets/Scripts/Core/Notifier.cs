using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public Canvas notificationCanvas;
    public TMP_Text notificationText;

    private float displayTime = 2f;

    private void Notify()
    {
        notificationCanvas.gameObject.SetActive(true);
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
        notificationCanvas.gameObject.SetActive(false);
    }
}
