using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyed;
    public GameObject HealthBar;

    public void ShowHealth()
    {
        HealthBar.SetActive(true);
    }

    public void HideHealth()
    {
        HealthBar.SetActive(false);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
