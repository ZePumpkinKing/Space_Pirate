using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;


    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public bool automatic;


    [Header("Reloading")]
    public int magCapacity;
    public float fireRateRPM;
    public float reloadTime;

    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;

    [HideInInspector] public float currentAmmo;
}
