using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private Text magCapacityText;
    [SerializeField] private Text currentAmmoText;
    [SerializeField] private RawImage pistolIndicator;
    [SerializeField] private RawImage shotgunIndicator;
    private Gun gun;

    private void Awake()
    {
        gun = FindObjectOfType<Gun>();
    }
    private void Update()
    {
        magCapacityText.text = gun.currentGun.magCapacity.ToString();
        currentAmmoText.text = gun.currentGun.currentAmmo.ToString();

        switch (gun.currentGun.name)
        {
            case "Pistol":
                shotgunIndicator.enabled = false;
                pistolIndicator.enabled = true;
                break;
            case "Shotgun":
                shotgunIndicator.enabled = true;
                pistolIndicator.enabled = false;
                break;
        }
    }
}
