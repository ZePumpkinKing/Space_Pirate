using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool immortal;
    public float health { get; private set; } = 100;
    public bool died;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }
    private void Update()
    {
        if (died)
        {
            StartCoroutine(StartDeathTimer());
            died = false;
        }
    }
    IEnumerator StartDeathTimer()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (!immortal) TakeDamage(10);
        }
    }
}
