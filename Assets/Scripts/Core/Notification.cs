using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    public TMP_Text notificationText;
    public Animator Animator;
    public AudioSource sxf;

    public void Notify(string message, bool playSound)
    {
        if (playSound) sxf.Play();
        notificationText.text = message;
        Animator.Play("ShowNotification");
        Destroy(gameObject, 1.3f);
    }
}
