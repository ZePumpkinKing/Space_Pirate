using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    GameManager gm;
    public float health;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log(health + " health remaining");
        if (health <= 0) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (gm.enteredRoom)
        {
            gm.UpdateEnemyCount();
        }
    }
}
