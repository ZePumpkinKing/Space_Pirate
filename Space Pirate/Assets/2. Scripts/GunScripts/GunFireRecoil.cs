using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireRecoil : MonoBehaviour
{
    //rotations
    public Vector3 currentRotation, targetRotation;
    public Vector3 initRot;
    Player playerScript;
    Gun gunScript;
    //recoil values

    private void Awake()
    {
        playerScript = FindObjectOfType<Player>();
        gunScript = FindObjectOfType<Gun>();
        initRot = transform.rotation.eulerAngles - playerScript.transform.rotation.eulerAngles;
    }
    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, initRot, gunScript.currentGun.returnSpeed * Time.deltaTime); ;
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gunScript.currentGun.snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void FireGunRecoil()
    {
        targetRotation -= new Vector3(gunScript.currentGun.recoilX,
            Random.Range(gunScript.currentGun.recoilY, -gunScript.currentGun.recoilY), Random.Range(gunScript.currentGun.recoilZ, -gunScript.currentGun.recoilZ));
    }
}
