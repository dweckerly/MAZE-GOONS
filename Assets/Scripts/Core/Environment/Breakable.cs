using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject vessel;
    public GameObject treasure;
    public ParticleSystem breakParticles;
    public Attributes attributes;

    private void Start() 
    {
        vessel.SetActive(true);
        if (treasure != null) treasure.SetActive(false);
    }

    private void OnEnable() 
    {
        attributes.OnDie += Break;    
    }

    private void OnDisable() 
    {
        attributes.OnDie -= Break;    
    }

    private void Break()
    {
        breakParticles.Play();
        vessel.SetActive(false);
        if (treasure != null) treasure.SetActive(true);
    }
}
