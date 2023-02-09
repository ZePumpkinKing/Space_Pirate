using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] GunData currentGun;
    [SerializeField] GunData[] guns;

    Input input;
    Transform castPoint;
    [SerializeField] private Transform gunTip;


    float timeSinceLastShot;
    float currentAmmo;
    bool reloading;
    float autoShooting;
    int activeGun;

    private Recoil recoilScript;
    private void Awake()
    {
        input = new Input();
        recoilScript = FindObjectOfType<Recoil>();
        castPoint = GameObject.FindGameObjectWithTag("CastPoint").GetComponent<Transform>();
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }

        input.Gameplay.Interact.performed += context => StartReload();
        input.Gameplay.Weapon.performed += context => SwitchWeapon();
    }
    private void Start()
    {
        currentAmmo = currentGun.magCapacity;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(castPoint.position, castPoint.forward);
        if (currentGun.automatic)
        {
            autoShooting = input.Gameplay.Fire.ReadValue<float>();
        }
        if (autoShooting > 0)
        {
            Shoot();
        }
    }
    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (currentGun.fireRateRPM / 60f);
    public void Shoot()
    {
        
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                recoilScript.FireRecoil();
                Debug.Log(currentAmmo);
                if (Physics.Raycast(castPoint.position, castPoint.forward, out RaycastHit hit, currentGun.maxDistance))
                {
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(currentGun.damage);
                }
                currentAmmo--;
                timeSinceLastShot = 0;
                //OnGunShot();
            }
        }

    }
    private void StartReload()
    {
        if (!reloading)
        {
            StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(currentGun.reloadTime);
        currentAmmo = currentGun.magCapacity;
        
        reloading = false;
    }

    private void SwitchWeapon()
    {
        switch(input.Gameplay.Weapon.ReadValue<int>())
        {
            case 1:
                break;

        }

        currentGun = guns[1];
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
