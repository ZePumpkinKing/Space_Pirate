using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableShipPart : MonoBehaviour
{
    [SerializeField] private GameObject shipPart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shipPart.SetActive(false);
        }
        
    }
}
