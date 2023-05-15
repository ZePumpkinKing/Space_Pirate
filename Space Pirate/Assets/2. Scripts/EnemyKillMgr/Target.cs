using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health;
    EnemyDoor[] doors;


    private void Awake()
    {
        doors = FindObjectsOfType<EnemyDoor>();
    }
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (this.gameObject.tag != "Projectile")
        {
            foreach (EnemyDoor door in doors)
            {
                door.UpdateEnemyCount();
            }
        }
        
    }

}
