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

    [SerializeField] private GameObject gunshotSparks;

    [SerializeField] private GameObject gunshotSparksEnemy;

    bool playSparks;
    private Animator anim;
    Player player;
    
    Input input;
    InputAction activeWeapon;

    //private internals
    Transform castPoint;

    private Recoil recoilScript;
    private GunFireRecoil gunObjRecoil;
    private GameObject currentGunObj;
    private int currentGunId;
    //floats
    float timeSinceLastShot;
    float autoShooting;
    //bools
    bool reloading;
    public bool switching;
    


    private void Awake()
    {
        player = FindObjectOfType<Player>();
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

        input.Gameplay.Reload.performed += context => StartReload();
        input.Gameplay.Weapon.performed += FindWeapon;
    }
    private void Start()
    {
        for (int i = 0; i < gunObjs.Length; i++) //loop through all of our current weapons in our inventory
        {
            guns[i].currentAmmo = guns[i].magCapacity; //set all gun data scriptable objects to be at their starting maximum ammo
            if (i > 0)
            {
                gunObjs[i].SetActive(false); //set only one gun to active
            }    
        }
    }
    private void Update()
    {
        Debug.Log(playSparks);
        if (player.currentState == Player.states.dead)
        {
            Destroy(gameObject.GetComponent<Gun>());
        }
        timeSinceLastShot += Time.deltaTime;
        if (currentGun.automatic)
        {
            autoShooting = input.Gameplay.Fire.ReadValue<float>(); // if the gun is automatic, when we hold down click, this will be equal to 1
        }
        if (autoShooting > 0) // if the gun is auto, autoshooting can be 1, and if it is 1, then shoot repetitively
        {
            Shoot();
        }
        if (reloading)
        {
            anim.SetBool("Reload", true); // sets our animation state
        } else anim.SetBool("Reload", false);

        if (timeSinceLastShot > currentGun.timeBetweenShots)
        {
            anim.SetBool("Firing", false);
        }

    }
    private bool CanShoot() => !reloading && timeSinceLastShot > currentGun.timeBetweenShots // if we're done firing the last shot
        && !switching; //if we aren't switching
    public void Shoot()
    {
        if (currentGun.currentAmmo > 0) // if we have ammo
        {
            if (CanShoot()) // if we are able to shoot, run the code to shoot
            {
                RaycastHit hit; //instantiate our raycast ref
                TrailRenderer trail; // instantiate our gun trail
                recoilScript.FireRecoil(); // camera recoil
                gunObjRecoil.FireGunRecoil(); // gun recoil

                StartCoroutine(PlayParticles(currentGun.muzzleParticle, gunTip.position, gunTip.rotation));
                
                anim.SetBool("Firing", true); //sets our animation
                
                for (int i = 0; i < currentGun.bulletsInOneShot; i++) //run the code to shoot for as many bullets as are supposed to shoot out of the gun (if we have a shotgun, we'll shoot 10 bullets thanks to this for loop)
                {
                    Vector3 direction = GetDirection(); //get the direction of our shot, adhering to our spread amount
                    if (Physics.Raycast(castPoint.position, direction, out hit, currentGun.maxDistance)) //if we hit an object with our bullet
                    {
                        trail = Instantiate(bulletTrail, gunTip.position, Quaternion.identity); //start a bullet trail effect
                        if (hit.point != Vector3.zero) playSparks = true;
                        StartCoroutine(SpawnBullet(trail, hit.point, hit)); //spawn our bullet
                    }
                    else // if we shoot, but we don't hit anything (if we shoot into the air at no objects, we still want to show our bullet trail)
                    {
                        trail = Instantiate(bulletTrail, gunTip.position, Quaternion.identity);
                        playSparks = false;
                        StartCoroutine(SpawnBullet(trail, castPoint.position + direction * (currentGun.maxDistance / 2), hit)); // sets the point of where our raycast would have ended up if it hit anything (point in the air)
                                           
                    }
                }

                //Debug.Log(hit.point);

                currentGun.currentAmmo--;
                timeSinceLastShot = 0;
            }
        } else StartReload();

    }

    private IEnumerator PlayParticles(GameObject particleParentObj, Vector3 particlePos, Quaternion particleRot)
    {
        
        var temp = Instantiate(particleParentObj, particlePos, particleRot);
        ParticleSystem[] particles;
        particles = temp.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem pp in particles)
        {
            pp.Play();
        }
        yield return new WaitForSeconds(5);
        Destroy(temp);
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

    private IEnumerator SpawnBullet(TrailRenderer trail, Vector3 hitPos, RaycastHit hit)
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
        if (damageable != null)
        {
            damageable?.TakeDamage(currentGun.damage);
            StartCoroutine(PlayParticles(gunshotSparksEnemy, hitPos, gunshotSparks.transform.rotation));
        }
        else if (currentGun != guns[1])
        {
            if (playSparks)
            {
                StartCoroutine(PlayParticles(gunshotSparks, hitPos, gunshotSparks.transform.rotation));
            }
            
        }

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
    public enum gunIDs
    {
        Pistol,//0

        Shotgun,//1

        Autogun,//2

        Blunderbus//3
    }
    private void FindWeapon(InputAction.CallbackContext context)
    {
        
        switch (activeWeapon.ReadValue<Vector2>().x)
        {
            case 1:// if we press 1

                StartCoroutine(SwitchWeapon((int)gunIDs.Pistol)); 
                break;

        }
        switch (activeWeapon.ReadValue<Vector2>().y)
        {
            case -1://if we press 2
                StartCoroutine(SwitchWeapon((int)gunIDs.Shotgun));
                break;
            case 120: //if we scroll up(don't know why unity sets this to 120)
                
                if (currentGunId + 1 <= guns.Length - 1)
                {
                    StartCoroutine(SwitchWeapon(currentGunId + 1));
                }
                break;
            case -120://if we scroll down
                if (currentGunId - 1 >= 0)
                {
                    StartCoroutine(SwitchWeapon(currentGunId - 1));
                }
                break;
        }
        
        if (!currentGun.automatic)
        {
            input.Gameplay.Fire.performed += context => Shoot();
        }
    }
    private IEnumerator SwitchWeapon(int gunId)
    {
        if (!reloading && !switching && timeSinceLastShot > currentGun.timeBetweenShots) //only switch guns if we arent currently reloading, or switching
        {
            if (currentGun != guns[gunId])
            {
                switching = true;
                SwitchGunModel(currentGunObj, gunObjs[gunId]);
                currentGunObj = gunObjs[gunId];
                currentGun = guns[gunId];
                currentGunId = (gunId);
                anim = currentGunObj.GetComponent<Animator>();
                yield return new WaitForSeconds(currentGun.readyUpTime);
                switching = false;
            }
            else yield return null;
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
