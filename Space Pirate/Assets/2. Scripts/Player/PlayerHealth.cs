using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool immortal;
    public float health { get; private set; } = 100;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }
    private void Update()
    {
        //Debug.Log(health);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (!immortal) TakeDamage(10);
        }
    }
}
