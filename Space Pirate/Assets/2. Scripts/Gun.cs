using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gun;
    Input input;
    Transform cam;
    [SerializeField] private Transform gunTip;

    float timeSinceLastShot;
    float currentAmmo;
    bool reloading;
    float autoShooting;
    private void Awake()
    {
        input = new Input();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        if (!gun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }

        input.Gameplay.Reload.performed += context => StartReload();
    }
    private void Start()
    {
        currentAmmo = gun.magCapacity;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
        if (gun.automatic)
        {
            autoShooting = input.Gameplay.Fire.ReadValue<float>();
        }
        if (autoShooting > 0)
        {
            Shoot();
        }
    }
    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (gun.fireRateRPM / 60f);
    public void Shoot()
    {
        
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                Debug.Log(currentAmmo);
                if (Physics.Raycast(new Vector3(cam.position.x, cam.position.y, cam.position.z + 1), cam.forward, out RaycastHit hit, gun.maxDistance))
                {
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gun.damage);
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
        yield return new WaitForSeconds(gun.reloadTime);
        currentAmmo = gun.magCapacity;
        
        reloading = false;
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
