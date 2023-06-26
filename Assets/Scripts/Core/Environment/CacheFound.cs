using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheFound : MonoBehaviour
{
    public bool found = false;
    
    private void OnTriggerEnter(Collider other)
    {
        found = true;
    }
}
