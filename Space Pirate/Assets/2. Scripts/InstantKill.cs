using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Test");
            //other.transform.gameObject.GetComponent<PlayerHealth>().TakeDamage(101f);
        }
    }
}
