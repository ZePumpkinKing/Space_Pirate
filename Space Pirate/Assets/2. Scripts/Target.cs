using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log(health + " health remaining");
        if (health <= 0) Destroy(this.gameObject);
    }
}
