using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [HideInInspector] public GunData currentGun;
    [SerializeField] GunData[] guns;
    [SerializeField] private GameObject[] gunObjs;
    private GameObject currentGunObj;
    Input input;
    Transform castPoint;
    [SerializeField] private Transform gunTip;
    private Animator anim;

    float timeSinceLastShot;
    bool reloading;
    float autoShooting;
    InputAction activeWeapon;

    private Recoil recoilScript;
    private void Awake()
    {
        gunObjs = GameObject.FindGameObjectsWithTag("Gun");
        
        currentGun = guns[0];
        currentGunObj = gunObjs[0];
        anim = currentGunObj.GetComponent<Animator>();
        input = new Input();
        recoilScript = FindObjectOfType<Recoil>();
        castPoint = GameObject.FindGameObjectWithTag("CastPoint").GetComponent<Transform>();
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }

        input.Gameplay.Interact.performed += context => StartReload();
        input.Gameplay.Weapon.performed += FindWeapon;
    }
    private void Start()
    {
        for (int i = 0; i < gunObjs.Length; i++) //
        {
            guns[i].currentAmmo = guns[i].magCapacity;
            if (i > 0)
            {
                SetInactive(gunObjs[i]);//set only one gun to active
            }    
        }


    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (currentGun.automatic)
        {
            autoShooting = input.Gameplay.Fire.ReadValue<float>();
        }
        if (autoShooting > 0)
        {
            Shoot();
        }
        if (reloading)
        {
            anim.SetBool("Reload", true);
        } else anim.SetBool("Reload", false);
    }
    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (currentGun.fireRateRPM / 60f);
    public void Shoot()
    {
        
        if (currentGun.currentAmmo > 0)
        {
            if (CanShoot())
            {
                recoilScript.FireRecoil();
                
                anim.SetTrigger("Firing");
                
                if (Physics.Raycast(castPoint.position, castPoint.forward, out RaycastHit hit, currentGun.maxDistance))
                {
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(currentGun.damage);
                }
                currentGun.currentAmmo--;
                timeSinceLastShot = 0;
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
        currentGun.currentAmmo = currentGun.magCapacity;
        
        reloading = false;
    }
    private void FindWeapon(InputAction.CallbackContext context)
    {
        switch(activeWeapon.ReadValue<Vector2>().x)
        {
            case 1:
                SwitchWeapon(0);
                break;
            case -1:
                SwitchWeapon(1);
                break;

        }
        switch(activeWeapon.ReadValue<Vector2>().y)
        {
            case 1:
                SwitchWeapon(2);
                break;
            case -1:
                SwitchWeapon(3);
                break;
        }
        
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }
    }
    private void SwitchWeapon(int gunId)
    {
        if (!reloading && timeSinceLastShot > 1f / (currentGun.fireRateRPM / 60f)) //only switch guns if we arent currently reloading, or on shot cooldown, prevents weird edge cases
        {
            SwitchGunModel(currentGunObj, gunObjs[gunId]);
            currentGunObj = gunObjs[gunId];
            currentGun = guns[gunId];
        }

    }
    private void SwitchGunModel(GameObject prevModel,GameObject newModel)
    {
        prevModel.SetActive(false);
        newModel.SetActive(true);
    }

    private void SetInactive(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnEnable()
    {
        input.Enable();
        activeWeapon = input.Gameplay.Weapon;
        activeWeapon.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        activeWeapon.Disable();
    }
}
