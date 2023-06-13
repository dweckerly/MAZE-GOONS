using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    public List<GameObject> EnableRooms;
    public List<GameObject> DisableRooms;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject go in EnableRooms)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in DisableRooms)
            {
                go.SetActive(false);
            }
        }  
    }
}
