using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliderHandler : MonoBehaviour
{
    [SerializeField] GameObject RightCollider;
    [SerializeField] GameObject LeftCollider;
    [SerializeField] GameObject HeadCollider;

    public void EnableRightCollider()
    {
        RightCollider.SetActive(true);
    }

    public void EnableLeftCollider()
    {
        LeftCollider.SetActive(true);
    }

    public void EnableHeadCollider()
    {
        HeadCollider.SetActive(true);
    }

    public void DisableRightCollider()
    {
        RightCollider.SetActive(false);
    }

    public void DisableLeftCollider()
    {
        LeftCollider.SetActive(false);
    }

    public void DisableHeadCollider()
    {
        HeadCollider.SetActive(false);
    }

}
