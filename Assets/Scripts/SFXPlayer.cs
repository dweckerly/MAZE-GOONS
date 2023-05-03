using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public List<AudioClip> footSteps;
    public AudioSource audioSource;

    void FootStep()
    {
        int r = Random.Range(0, footSteps.Count);
        audioSource.clip = footSteps[r];
        audioSource.Play();
    }
    
}
