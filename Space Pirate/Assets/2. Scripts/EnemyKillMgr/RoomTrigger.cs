using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    GameManager gm;
    private bool thisEntered;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!gm.enteredRoom && !thisEntered)
            {
                Debug.Log("Enabled trigger");
                thisEntered = true;
                gm.enteredRoom = true;
                gm.roomNumber++;
            }
        }
    }
}
