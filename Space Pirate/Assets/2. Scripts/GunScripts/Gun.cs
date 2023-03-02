using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [HideInInspector] public GunData currentGun;

    [Header("References")]
    [SerializeField] GunData[] guns;

    [SerializeField] private GameObject[] gunObjs;

    [SerializeField] private TrailRenderer bulletTrail;

    [SerializeField] private LayerMask canHit;
    
    [SerializeField] private Transform gunTip;

    private Animator anim;
    
    Input input;
    InputAction activeWeapon;

    //private internals
    Transform castPoint;

    private Recoil recoilScript;
    private GunFireRecoil gunObjRecoil;
    private GameObject currentGunObj;
    //floats
    float timeSinceLastShot;
    float autoShooting;
    //bools
    bool reloading;
    public bool switching;
    


    private void Awake()
    {
        gunObjs = GameObject.FindGameObjectsWithTag("Gun");
        gunObjRecoil = FindObjectOfType<GunFireRecoil>();
        
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
                gunObjs[i].SetActive(false); //set only one gun to active
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
    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (currentGun.fireRateRPM / 60f) && !switching;
    public void Shoot()
    {
        if (currentGun.currentAmmo > 0)
        {
            if (CanShoot())
            {
                RaycastHit hit;
                TrailRenderer trail;
                recoilScript.FireRecoil();
                gunObjRecoil.FireGunRecoil();
                
                anim.SetTrigger("Firing");
                
                for (int i = 0; i < currentGun.bulletsInOneShot; i++)
                {
                    Vector3 direction = GetDirection();
                    if (Physics.Raycast(castPoint.position, direction, out hit, currentGun.maxDistance))
                    {
                        Debug.Log(hit.point);
                        trail = Instantiate(bulletTrail, gunTip.position, Quaternion.identity);
                        StartCoroutine(SpawnTrail(trail, hit.point, hit));
                    }
                    else
                    {
                        Debug.Log(castPoint.position + castPoint.transform.forward);
                        trail = Instantiate(bulletTrail, gunTip.position, Quaternion.identity);
                        StartCoroutine(SpawnTrail(trail, castPoint.position + direction * (currentGun.maxDistance / 2), hit));
                    }
                }

                //Debug.Log(hit.point);

                currentGun.currentAmmo--;
                timeSinceLastShot = 0;
            }
        }

    }

    private Vector3 GetDirection()
    {
        Vector3 direction = castPoint.forward;

        direction += new Vector3(Random.Range(-currentGun.spreadX, currentGun.spreadX),
                                 Random.Range(-currentGun.spreadY, currentGun.spreadY),
                                 Random.Range(-currentGun.spreadZ, currentGun.spreadZ));
        direction.Normalize();
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPos, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPos, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hitPos;
        IDamageable damageable = hit.transform.GetComponent<IDamageable>();
        damageable?.TakeDamage(currentGun.damage);
        Destroy(trail.gameObject, trail.time);
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
    enum gunIDs
    {
        Pistol,//0

        Shotgun,//1

        Autogun,//2

        Blunderbus//3
    }
    private void FindWeapon(InputAction.CallbackContext context)
    {

        switch(activeWeapon.ReadValue<Vector2>().x)
        {
            case 1:// if we press 1
                StartCoroutine(SwitchWeapon(gunIDs.Pistol)); 
                break;
            case -1://if we press 3
                StartCoroutine(SwitchWeapon(gunIDs.Autogun));
                break;

        }
        switch(activeWeapon.ReadValue<Vector2>().y)
        {
            case 1://if we press 4
                StartCoroutine(SwitchWeapon(gunIDs.Blunderbus));
                break;
            case -1://if we press 2
                StartCoroutine(SwitchWeapon(gunIDs.Shotgun));
                break;
        }
        
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }
    }
    private IEnumerator SwitchWeapon(gunIDs gunId)
    {
        if (!reloading && !switching && timeSinceLastShot > 1f / (currentGun.fireRateRPM / 60f)) //only switch guns if we arent currently reloading, or switching
        {
            switching = true;
            anim.SetTrigger("Stowed");
            yield return new WaitForSeconds(1);
            SwitchGunModel(currentGunObj, gunObjs[((int)gunId)]);
            currentGunObj = gunObjs[((int)gunId)];
            currentGun = guns[((int)gunId)];
            yield return new WaitForSeconds(1);
            switching = false;
        }
        else yield return null;

    }
    private void SwitchGunModel(GameObject prevModel,GameObject newModel)
    {
        prevModel.SetActive(false);
        newModel.SetActive(true);
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
