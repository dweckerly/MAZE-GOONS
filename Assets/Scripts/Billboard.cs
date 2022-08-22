using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCam;
    
    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCam.transform);
    }
}
