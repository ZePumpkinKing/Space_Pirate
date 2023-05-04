using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool immortal;
    public float health { get; private set; } = 100;
    public bool died;
    float secondsUntilRegen = 7f;
    float timer;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        timer = 0;
    }
    private void Update()
    {
        if (died)
        {
            StartCoroutine(StartDeathTimer());
            died = false;
        }
        if (health < 100)
        {
            timer += Time.deltaTime;
            if (timer > secondsUntilRegen)
            {
                health += Time.deltaTime * 3;
            }
        }
        if (health > 100)
        {
            health = 100;
        }
    }
    IEnumerator StartDeathTimer()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (!immortal) 
            {
                var energyRef = other?.GetComponent<EnergyBall>();
                var rocketRef = other?.GetComponent<Rocket>();
                var laserRef = other?.GetComponent<Laser>();
                if (rocketRef != null)
                {
                    Debug.Log("Subtracting rocket dmg");
                    TakeDamage(rocketRef.damage);
                } else if (energyRef != null)
                {
                    Debug.Log("Subtracting energy ball dmg");
                    TakeDamage(energyRef.damage);
                } else if (laserRef != null)
                {
                    //TakeDamage(laserRef.damage);
                }
 
            }
        }
    }
}
