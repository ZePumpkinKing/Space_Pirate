using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gun;
    Input input;
    Transform cam;

    float timeSinceLastShot;
    float currentAmmo;
    bool reloading;
    private void Awake()
    {
        input = new Input();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        input.Gameplay.Fire.performed += context => Shoot();
    }
    private void Start()
    {
        currentAmmo = gun.magCapacity;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward);
    }
    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (gun.fireRateRPM / 60f);
    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(new Vector3(cam.position.x, cam.position.y, cam.position.z + 1), cam.forward, out RaycastHit hit, gun.maxDistance))
                {
                    Debug.Log(hit.transform.name);
                }
                currentAmmo--;
                timeSinceLastShot = 0;
                //OnGunShot();
            }
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
