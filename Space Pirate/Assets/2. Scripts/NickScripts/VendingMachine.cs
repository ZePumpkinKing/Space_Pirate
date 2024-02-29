using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IDamageable
{
    public GameObject[] sodaCans;

    public int canID;

    public GameObject selectedCan;

    public Transform spawnPoint;

    ///public Vector3 canScale;

    void Start()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        int canID = Random.Range(0, 3);
        selectedCan = sodaCans[canID];

        ///selectedCan.transform.localScale = canScale;
        Instantiate(selectedCan, spawnPoint);
    }
}
